using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUI : MonoBehaviour
{
    [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
    [BoxGroup("BaseUI")]
    [SerializeField] bool _isSubCanvas = false;
    public bool IsSubCanvas => _isSubCanvas;

    public event Action<BaseUI> OnOpenAction;
    public event Action<BaseUI> OnCloseAction;
    public event Action<BaseUI> OnDestroyAction;

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
        
        Logger.Log($"{this.gameObject.name} Open" );
        OnOpenAction?.Invoke(this);
        
        OpenUIInternal();
    }

    public void CloseUI()
    {
        // todo 뿅하고 꺼진다던가
        OnCloseAction?.Invoke(this);
        CloseUIInternal();
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        OnDestroyAction?.Invoke(this);
    }


    public virtual void OpenUIInternal() { }
    public virtual void CloseUIInternal() { }



}
