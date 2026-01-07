using System;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    public event Action<BaseUI> OnOpenAction;
    public event Action<BaseUI> OnCloseAction;
    public event Action<BaseUI> OnDestroyAction;
    
    
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
        this.gameObject.SetActive(false);
        
        OnCloseAction?.Invoke(this);
        
        CloseUIInternal();
    }

    private void OnDestroy()
    {
        OnDestroyAction?.Invoke(this);
    }


    public virtual void OpenUIInternal() { }
    public virtual void CloseUIInternal() { }



}
