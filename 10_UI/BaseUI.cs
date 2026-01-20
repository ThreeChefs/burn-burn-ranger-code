using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    [BoxGroup("BaseUI")]
    [SerializeField] bool _isSubCanvas = false;

    [ShowIf("_isSubCanvas")]
    [BoxGroup("BaseUI")]
    [HideLabel]
    [EnumToggleButtons]
    [SerializeField] private UISubCanvasOrder _subUIOrder;
    public UISubCanvasOrder SubUIOrder => _subUIOrder;


    public bool IsSubCanvas => _isSubCanvas;

    public event Action<BaseUI> OnOpenAction;
    public event Action<BaseUI> OnDestroyAction;
    public event Action<BaseUI> OnClosedAction;


    private void Awake()
    {
        AwakeInternal();
    }

    protected virtual void AwakeInternal()
    {

    }

    // todo 애니메이션 넣게 되면 UI 이벤트 안되게 처리도 필요
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
