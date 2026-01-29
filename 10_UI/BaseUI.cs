using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;


public abstract class BaseUI : MonoBehaviour
{
    [BoxGroup("BaseUI")]



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

    public event Action<BaseUI> OnCloseAction;
    public event Action<BaseUI> OnClosedAction;

    protected Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();

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
        OnCloseAction?.Invoke(this);

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
