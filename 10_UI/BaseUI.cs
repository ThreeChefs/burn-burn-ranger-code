using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    public event Action OnEnableAction;
    public event Action OnDisableAction;
    
    void OnEnable()
    {
        OnEnableAction?.Invoke();
    }
    
    

    void OnDisable()
    {
        OnDisableAction?.Invoke();
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
    
}
