using System;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    public event Action OnEnableAction;
    public event Action OnDisableAction;
    
    void OnEnable()
    {
        OnEnableAction?.Invoke();
        OnEnableInternal();
    }

    void OnDisable()
    {
        OnDisableAction?.Invoke();
        OnDisableInternal();
    }
    
    // todo 애니메이션 넣게 되면 UI 이벤트 안되게 처리도 필요
    public void OpenUI()
    {
        this.gameObject.SetActive(true);
    }

    public void CloseUI()
    {
        // todo 뿅하고 꺼진다던가
        this.gameObject.SetActive(false);
    }

    public abstract void OnEnableInternal();
    public abstract void OnDisableInternal();



}
