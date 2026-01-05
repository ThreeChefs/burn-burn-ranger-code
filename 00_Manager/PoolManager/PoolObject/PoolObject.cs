using System;
using UnityEngine;

/// <summary>
/// 풀링할 게임오브젝트가 해당 컴포넌트를 가지고 있으면 잘 동작합니다.
/// 해당 오브젝트의 메인이 될 컴포넌트가 상속받아서 사용하셔도 됩니다!
/// 따로 추가 컴포넌트로 넣고 이벤트를 넣어주시면 됩니다!
/// 아니면 PoolObject를 상속해서 추가로 만드시고 별도로 초기화를 하셔도 됩니다. 풀링으로 재활용이 잘 돌아가면 되고 사용은 자유롭게!
/// </summary>
public class PoolObject : MonoBehaviour
{
    public event Action<PoolObject> OnEnableAction;
    public event Action<PoolObject> OnDisableAction;


    private void OnEnable()
    {
        OnEnableAction?.Invoke(this);
        OnEnableInternal();
    }

    protected virtual void OnEnableInternal()
    {
        
    }

    private void OnDisable()
    {
        OnDisableInternal();
        OnDisableAction?.Invoke(this);
    }

    protected virtual void OnDisableInternal()
    {
        
    }
}