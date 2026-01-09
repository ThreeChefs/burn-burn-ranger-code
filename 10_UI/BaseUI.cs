using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUI : MonoBehaviour
{
    [BoxGroup("BaseUI")]
    [SerializeField] bool _isSubCanvas = false;

    [ShowIf("_isSubCanvas")]
    [BoxGroup("BaseUI")]
    [HideLabel][EnumToggleButtons]
    [SerializeField] private UISubCanvasOrder _subUIOrder;


    public bool IsSubCanvas => _isSubCanvas;

    public event Action<BaseUI> OnOpenAction;
    public event Action<BaseUI> OnCloseAction;
    public event Action<BaseUI> OnDestroyAction;

    private void Awake()
    {
        Canvas canvas = this.gameObject.GetComponent<Canvas>();
        if(_isSubCanvas && canvas != null)
        {
            canvas.sortingOrder = (int)_subUIOrder;
        }

        AwakeInternal();
    }
    
    protected virtual void AwakeInternal()
    {

    }

    // todo 애니메이션 넣게 되면 UI 이벤트 안되게 처리도 필요
    public void OpenUI()
    {
        this.gameObject.SetActive(true);
        
        Logger.Log($"{this.gameObject.name} Open" );
        OnOpenAction?.Invoke(this);
        
        OpenUIInternal();
    }

    public void CloseUI()
    {
        // todo 뿅하고 꺼진다던가
        OnCloseAction?.Invoke(this);

        Tween closeTween = CloseUIInternal();

        if (closeTween != null)
        {
            CloseUIInternal().OnComplete(() =>
            {
                this.gameObject.SetActive(false);
            });
            return;
        }
        
        this.gameObject.SetActive(false);
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
