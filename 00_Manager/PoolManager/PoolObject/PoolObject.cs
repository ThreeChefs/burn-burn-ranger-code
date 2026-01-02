using System;
using UnityEngine;

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