using DG.Tweening;
using System;
using Unity.VisualScripting;
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
    [Header("컴포넌트")]
    // 아이템 범위 
    [SerializeField] private CircleCollider2D _gemCollector;
    private float _defaultRadius;

    // hp바
    [SerializeField] private Transform _hpBarPivot;

    // 이미지
    [SerializeField] private Transform _moveDirectionArrow;
    [SerializeField] private float _rotateDuration = 0.75f;

    [SerializeField] private float _bloodParticleOffset = 1f;
    [SerializeField] private SpriteRenderer[] _renderers;
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

    // 액티브 스킬 컨테이너
    [field: SerializeField] public Transform SkillContainer { get; private set; }

    // 캐싱
    public PlayerCondition Condition { get; private set; }
    private PlayerStat _health;
    private PlayerStat _heal;

    // 타이머
    private float _healTimer;

    // 레벨
    public LevelSystem StageLevel { get; private set; }

    // 골드
    public int GoldValue { get; private set; }

    // 움직임
    private PlayerStat _speed;
    private Vector2 _inputVector;

    // 이벤트
    public event Action OnDieAction;
    #endregion

    #region Unity API
    private void Awake()
    {
        StageLevel = new(1, 0f);
        GoldValue = 0;
        _defaultRadius = _gemCollector.radius;
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
    }

    private void Update()
    {
        _healTimer += Time.deltaTime;
        if (_heal.MaxValue != 0 && _healTimer > Define.HealTime)
        {
            _health.Add(_health.MaxValue * _heal.MaxValue);
            _healTimer = 0f;
        }

        float angle = Mathf.Atan2(_inputVector.y, _inputVector.x) * Mathf.Rad2Deg;
        _moveDirectionArrow.DORotate(new Vector3(0, 0, angle), _rotateDuration);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnDestroy()
    {
        StageLevel.OnDestroy();
        Condition[StatType.DropItemRange].OnMaxValueChanged -= OnUpdateColliderSize;
    }
    #endregion

    #region Collider 관리
    private void OnUpdateColliderSize(float radius)
    {
        _gemCollector.radius = _defaultRadius * radius;
    }
    #endregion

    #region Move
    private void Move()
    {
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
        if (context.phase == InputActionPhase.Performed)
        {
            _inputVector = context.ReadValue<Vector2>();
            _moveDirectionArrow.gameObject.SetActive(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _inputVector = Vector2.zero;
            _moveDirectionArrow.gameObject.SetActive(false);
        }
    }
    #endregion

    #region sprite 관리
    private void Flip()
    {
        foreach (SpriteRenderer renderer in _renderers)
        {
            renderer.flipX = IsLeft;
        }
    }
    #endregion

    public void TakeDamage(float value)
    {
        PlayerStat health = Condition[StatType.Health];
        if (health.TryUse(value * (1 - Condition[StatType.DamageReduction].MaxValue)))
        {
            CommonPoolManager.Instance.Spawn(CommonPoolIndex.Particle_Blood, transform.position + Vector3.up * _bloodParticleOffset);
            if (health.CurValue == 0)
            {
                OnDieAction?.Invoke();
            }
        }
        else
        {
            OnDieAction?.Invoke();
        }
    }

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
            _gemCollector.AddComponent<GemCollector>();
        }
        _gemCollector.radius = 0.5f;
        _hpBarPivot = transform.FindChild<Transform>("HpBarPivot");
        _moveDirectionArrow = transform.FindChild<Transform>("MoveDirectionArrow");
        _renderers = transform.FindChild<Transform>("Model").GetComponentsInChildren<SpriteRenderer>();
        SkillContainer = transform.FindChild<Transform>("SkillContainer");
    }
#endif
    #endregion
}
