using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUI : MonoBehaviour
{
    [BoxGroup("BaseUI")]

    [BoxGroup("BaseUI/캔버스 설정")]
    [SerializeField] bool _isSelfCanvas = false;
    public bool IsSelfCanvas => _isSelfCanvas;

    [BoxGroup("BaseUI/캔버스 설정")]
    [ShowIf("_isSelfCanvas")]
    [SerializeField]
    [Range(0f, 1f)]
    private float _matchSize = 0f;
    public float MatchSirenSize => _matchSize;



    [BoxGroup("BaseUI/UI 순서")]
    [EnumToggleButtons]
    [SerializeField] private UICanvasOrder _subUIOrder;
    public UICanvasOrder UIOrder => _subUIOrder;

    [BoxGroup("BaseUI/UI 순서")]
    [Range(0, 99)]
    [SerializeField]
    private int _customOrder = 0;


    public event Action<BaseUI> OnOpenAction;
    public event Action<BaseUI> OnDestroyAction;
    public event Action<BaseUI> OnClosedAction;


    Canvas _canvas;
    CanvasScaler _scaler;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();

        if (IsSelfCanvas)
        {
            if (_canvas == null)
            {
                _canvas = this.gameObject.AddComponent<Canvas>();
            }

            _canvas.sortingOrder = (int)_subUIOrder;


            _scaler = GetComponent<CanvasScaler>();
            if (_scaler == null)
            {
                _scaler = this.gameObject.AddComponent<CanvasScaler>();
            }

            _scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _scaler.referenceResolution = new Vector2(1080f, 1920f);
            _scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            _scaler.matchWidthOrHeight = 0f;
        }
        else
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        if (_customOrder > 0)
        {
            _canvas.overrideSorting = true;
            _canvas.sortingOrder = (int)_subUIOrder + _customOrder;
        }

        AwakeInternal();
    }

    protected virtual void AwakeInternal()
    {

    }

    public void OpenUI()
    {
        this.gameObject.SetActive(true);

        OnOpenAction?.Invoke(this);

        OpenUIInternal();
    }

    public void CloseUI()
    {
        Tween closeTween = CloseUIInternal();

        if (closeTween != null)
        {
            closeTween.OnComplete(() =>
            {
                this.gameObject.SetActive(false);
                OnClosedAction?.Invoke(this);
            });

            return;
        }

        this.gameObject.SetActive(false);
        OnClosedAction?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnDestroyAction?.Invoke(this);
    }


    public virtual void OpenUIInternal() { }
    public virtual Tween CloseUIInternal()
    {
        return null;
    }





}
