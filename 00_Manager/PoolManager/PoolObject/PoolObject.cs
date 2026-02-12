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
    public event Action<PoolObject> OnEnableAction;     // 활성화 될 때 호출되는 이벤트
    public event Action<PoolObject> OnDisableAction;    // 비활성화 될 때 호출되는 이벤트
    public event Action<PoolObject> OnDestroyAction;    // 파괴 될 때 호출되는 이벤트

    /// <summary>
    /// PoolObject 가 Create 될 때 초기화할 내용을 넣어 주세요
    /// </summary>
    // PoolObject 가 생성될 때 호출되는 초기화 함수
    public virtual void InitPoolObject(){}

    // PoolObject 가 활성화 될 때 호출되는 함수
    private void OnEnable()
    {
        OnEnableAction?.Invoke(this);
        OnEnableInternal();
    }

    // PoolObject 가 활성화 될 때 내부에서 처리할 내용이 있으면 여기에 작성
    protected virtual void OnEnableInternal(){}

    // PoolObject 가 비활성화 될 때 호출되는 함수
    private void OnDisable()
    {
        OnDisableInternal();
        OnDisableAction?.Invoke(this);
        this.transform.localScale = Vector3.one;
    }

    // PoolObject 가 비활성화 될 때 내부에서 처리할 내용이 있으면 여기에 작성
    protected virtual void OnDisableInternal(){}

    // PoolObject 가 파괴될 때 호출되는 함수
    private void OnDestroy()
    {
        OnDestroyInternal();
        OnDestroyAction?.Invoke(this);
    }

    // PoolObject 가 파괴될 때 내부에서 처리할 내용이 있으면 여기에 작성
    protected void OnDestroyInternal(){}
}