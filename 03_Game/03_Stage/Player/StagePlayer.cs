using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 스테이지 내부 플레이어
/// </summary>
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerInput))]
public class StagePlayer : MonoBehaviour, IDamageable
{
    #region 필드
    [Title("Collider")]
    // 아이템 범위 
    [SerializeField] private CircleCollider2D _gemCollector;
    private float _defaultRadius;

    [Title("UI")]
    // hp바
    [SerializeField] private Transform _hpBarPivot;

    [Title("Sprs")]
    // 이미지
    [SerializeField] private Transform _moveDirectionArrow;
    [SerializeField] private float _rotateDuration = 0.75f;
    private float _lastAngle;

    [SerializeField] private float _bloodParticleOffset = 1f;
    [SerializeField] private SpriteRenderer[] _sprs;
    [SerializeField] private Color _hitColor = Color.red;
    [SerializeField] private float _flashDuration = 0.4f;
    private bool _isLeft;
    protected bool IsLeft
    {
        get { return _isLeft; }
        set
        {
            if (_isLeft != value)
            {
                _isLeft = value;
                Flip();
            }
        }
    }
    private Color[] _originColors;

    // 애니메이션
    private Animator _animator;
    private PlayerAnimation _playerAnimation;

    [field: Title("Skill")]
    // 액티브 스킬 컨테이너
    [field: SerializeField] public Transform SkillContainer { get; private set; }

    // 조이스틱
    private JoyStickInput _joyStick;

    // 캐싱
    public PlayerCondition Condition { get; private set; }
    private PlayerStat _health;
    private PlayerStat _heal;

    // 장비 효과
    private List<EquipmentEffectInstance> _effects;
    public IReadOnlyList<EquipmentEffectInstance> Effects => _effects;
    public BuffSystem BuffSystem { get; private set; }

    // 기록
    public KillStatus KillStatus { get; private set; }
    public DamageStatus DamageStatus { get; private set; }

    // 타이머
    private float _healTimer;

    // 레벨
    public LevelSystem StageLevel { get; private set; }

    // 골드
    public int GoldValue { get; private set; }

    // 움직임
    private PlayerStat _speed;
    private Vector2 _inputVector;

    // tween
    private Tween _arrowRotateTween;
    private Sequence _hitFlashSequence;

    // 이벤트
    public event Action OnDieAction;
    #endregion

    #region Unity API
    private void Awake()
    {
        StageLevel = new(1, 0f);
        GoldValue = 0;
        _defaultRadius = _gemCollector.radius;
        _effects = new();

        _animator = GetComponentInChildren<Animator>();
        _playerAnimation = new(_animator);

        _originColors = new Color[_sprs.Length];
        for (int i = 0; i < _originColors.Length; i++)
        {
            _originColors[i] = _sprs[i].color;
        }

        DamageStatus = new();
    }

    private void Start()
    {
        Condition = PlayerManager.Instance.Condition;
        _speed = Condition[StatType.Speed];
        _health = Condition[StatType.Health];
        _heal = Condition[StatType.Heal];
        _health.ResetCurValue(true);

        Condition[StatType.DropItemRange].OnMaxValueChanged += OnUpdateColliderSize;

        // hp 바 연결
        StatSliderUI statSliderUI = UIManager.Instance.SpawnWorldUI(UIName.WorldUI_Hp, _hpBarPivot) as StatSliderUI;
        statSliderUI.Init(_health);

        _moveDirectionArrow.gameObject.SetActive(false);

        _joyStick = UIManager.Instance.LoadUI(UIName.UI_JoyStick) as JoyStickInput;

        InitEquipmentEffects();
    }

    private void Update()
    {
        HandleHeal();
        UpdateArrow();
        BuffSystem.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnDisable()
    {
        _arrowRotateTween?.Kill();
        _hitFlashSequence?.Kill();
    }

    private void OnDestroy()
    {
        StageLevel.OnDestroy();
        Condition[StatType.DropItemRange].OnMaxValueChanged -= OnUpdateColliderSize;
        Condition.ResetBuff();
    }
    #endregion

    /// <summary>
    /// 장비 아이템으로 인해 초기화 해야 하는 부분
    /// </summary>
    private void InitEquipmentEffects()
    {
        BuffSystem = new(Condition);

        // 장비 effect data 가져와서 effect instance 생성하기
        foreach (BaseEquipmentEffectSO effectSO in PlayerManager.Instance.Equipment.EffectSOs)
        {
            Logger.Log($"스킬 효과 생성: {effectSO.name}");
            EquipmentEffectInstance instance = effectSO.CreateInstance();
            _effects.Add(instance);
        }

        foreach (EquipmentEffectInstance instance in _effects)
        {
            instance.OnStageStart(BuffSystem);
        }

        KillStatus = new();
        KillStatus.Init();
    }

    #region Collider 관리
    private void OnUpdateColliderSize(float radius)
    {
        _gemCollector.radius = _defaultRadius * radius;
    }
    #endregion

    private void HandleHeal()
    {
        _healTimer += Time.deltaTime;
        if (_heal.MaxValue != 0 && _healTimer > Define.HealTime)
        {
            _health.Add(_health.MaxValue * _heal.MaxValue);
            BuffSystem.OnHpChanged(_health.CurValue / _health.MaxValue);
            _healTimer = 0f;
        }
    }

    private void UpdateArrow()
    {
        if (_inputVector.sqrMagnitude < 0.001f)
        {
            _moveDirectionArrow.gameObject.SetActive(false);
            _arrowRotateTween?.Kill();
            _arrowRotateTween = null;
            return;
        }

        _moveDirectionArrow.gameObject.SetActive(true);

        float angle = Mathf.Atan2(_inputVector.y, _inputVector.x) * Mathf.Rad2Deg;

        // 각도 변화 없으면 갱신 안 함
        if (Mathf.Abs(Mathf.DeltaAngle(_lastAngle, angle)) < 0.1f)
            return;

        _lastAngle = angle;

        _arrowRotateTween?.Kill();
        _arrowRotateTween = _moveDirectionArrow
            .DORotate(new Vector3(0, 0, angle), _rotateDuration)
            .SetEase(Ease.OutCubic);
    }

    #region Move
    private void Move()
    {
        if (Define.EnableMobileUI)
        {
            _inputVector = _joyStick.Direction;
        }

        if (_inputVector != Vector2.zero)
        {
            _playerAnimation.Run();
        }
        else
        {
            _playerAnimation.Idle();
        }

        Vector2 nextVec = _speed.MaxValue * Time.fixedDeltaTime * _inputVector.normalized;
        if (nextVec.x != 0)
        {
            IsLeft = nextVec.x > 0;
        }
        Vector2 pos = transform.position;
        Vector2 newPos = pos + nextVec;
        transform.position = newPos;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _inputVector = Vector2.zero;
            return;
        }

        _inputVector = context.ReadValue<Vector2>();
    }
    #endregion

    #region 시각 연출 관리
    private void Flip()
    {
        foreach (SpriteRenderer renderer in _sprs)
        {
            renderer.flipX = IsLeft;
        }
    }

    private void HitVfx()
    {
        CommonPoolManager.Instance.Spawn(
            CommonPoolIndex.Particle_Blood,
            transform.position + Vector3.up * _bloodParticleOffset);

        _hitFlashSequence?.Kill();
        _hitFlashSequence = DOTween.Sequence();

        for (int i = 0; i < _sprs.Length; i++)
        {
            _hitFlashSequence.Join(_sprs[i].DOColor(_hitColor, _flashDuration));
        }

        _hitFlashSequence.AppendInterval(_flashDuration);

        for (int i = 0; i < _sprs.Length; i++)
        {
            _hitFlashSequence.Join(_sprs[i].DOColor(_originColors[i], _flashDuration));
        }
    }
    #endregion

    #region 전투 관리
    public void TakeDamage(float value)
    {
        if (_health.TryUse(value * (1 - Condition[StatType.DamageReduction].MaxValue)))
        {
            HitVfx();
            BuffSystem.OnPlayerHit();
            BuffSystem.OnHpChanged(_health.CurValue / _health.MaxValue);

            if (_health.CurValue == 0)
            {
                Die();
            }
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        foreach (var effect in _effects.OfType<IReviveEffect>())
        {
            BaseEffectContext context = new()
            {
                Reason = TriggerReason.Hpzero,
                BuffSystem = BuffSystem
            };

            if (!effect.CanTrigger(context))
            {
                continue;
            }

            Logger.Log("부활!!!!!");
            ReviveData data = effect.GetReviveData();
            Revive(data);
            return;
        }
        _playerAnimation.Die();
        OnDieAction?.Invoke();
    }

    private void Revive(ReviveData data)
    {
        _health.Add(_health.MaxValue * data.hpRatio);
        // todo: 무적 만들기
    }
    #endregion

    #region 레벨
    public void AddExp(float exp)
    {
        StageLevel.AddExp(exp * Condition[StatType.AddEXP].MaxValue);
    }
    #endregion

    #region 골드
    public void AddGold(int amount)
    {
        GoldValue += Mathf.FloorToInt(amount * Condition[StatType.AddGold].MaxValue);
    }

    /// <summary>
    /// [public] 스테이지 종료 시 획득한 골드 저장하기
    /// </summary>
    public void UpdateGold()
    {
        PlayerManager.Instance.Wallet[WalletType.Gold].Add(GoldValue);
    }
    #endregion

    #region 에디터 전용
#if UNITY_EDITOR
    private void Reset()
    {
        TryGetComponent<Rigidbody2D>(out var rigidbody2D);
        rigidbody2D.gravityScale = 0;

        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        collider.offset = new Vector2(-0.03598577f, 0.2159152f);
        collider.size = new Vector2(0.805023f, 1.287887f);

        string player = "Player";
        PlayerInput input = GetComponent<PlayerInput>();
        input.actions = AssetLoader.FindAndLoadByName<InputActionAsset>(player);
        input.defaultActionMap = player;
        input.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
        foreach (PlayerInput.ActionEvent actionEvent in input.actionEvents)
        {
            if (actionEvent.actionId == "Move")
            {
                actionEvent.AddListener(OnMove);
            }
        }

        // serialize field 연결
        _gemCollector = transform.FindChild<CircleCollider2D>("GemCollector");
        if (!_gemCollector.TryGetComponent<GemCollector>(out var gemCollector))
        {
            _gemCollector.gameObject.AddComponent<GemCollector>();
        }
        _gemCollector.radius = 0.5f;
        _hpBarPivot = transform.FindChild<Transform>("HpBarPivot");
        _moveDirectionArrow = transform.FindChild<Transform>("MoveDirectionArrow");
        _sprs = transform.FindChild<Transform>("Model").GetComponentsInChildren<SpriteRenderer>();
        SkillContainer = transform.FindChild<Transform>("SkillContainer");
    }
#endif
    #endregion
}
