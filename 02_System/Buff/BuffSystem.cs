using System.Collections.Generic;

/// <summary>
/// 버프 시스템
/// </summary>
public class BuffSystem
{
    private readonly List<BuffInstance> _active = new();
    private readonly PlayerCondition _condition;

    public BuffSystem(PlayerCondition condition)
    {
        _condition = condition;
    }

    #region 버프 추가 / 삭제
    /// <summary>
    /// [public] 키 값 확인하고 버프 추가하기
    /// </summary>
    /// <param name="key"></param>
    /// <param name="buff"></param>
    public void Add(BuffInstanceKey key, BaseBuff buff)
    {
        BuffInstance existing = null;
        for (int i = 0; i < _active.Count; i++)
        {
            if (_active[i].Key.Equals(key))
            {
                existing = _active[i];
            }
        }

        if (existing != null)
        {
            ResolveStack(existing, buff);
            return;
        }

        BuffInstance instance = new(key, buff);
        _active.Add(instance);
        instance.Activate(_condition);
    }

    /// <summary>
    /// 버프 삭제
    /// </summary>
    /// <param name="instance"></param>
    private void Remove(BuffInstance instance)
    {
        instance.Deactive(_condition);
        _active.Remove(instance);
    }

    /// <summary>
    /// 버프가 중첩되었을 때 StackPolicy에 따라 처리하기
    /// </summary>
    /// <param name="existing"></param>
    /// <param name="incoming"></param>
    private void ResolveStack(BuffInstance existing, BaseBuff incoming)
    {
        switch (incoming.StackPolicy)
        {
            case BuffStackPolicy.Refresh:
                existing.Refresh();
                break;
            case BuffStackPolicy.Stack:
                existing.StackTime(incoming.BaseDuration);
                break;
            case BuffStackPolicy.Replace:
                Remove(existing);
                Add(existing.Key, incoming);
                break;
            case BuffStackPolicy.Ignore:
                break;
        }
    }
    #endregion

    #region 조건 관리
    /// <summary>
    /// [public] 버프 시간 관리
    /// </summary>
    /// <param name="dt"></param>
    public void Update(float dt)
    {
        for (int i = _active.Count - 1; i >= 0; i--)
        {
            var instance = _active[i];
            instance.Tick(dt);

            if (instance.IsExpired)
            {
                Remove(instance);
            }
        }
    }

    /// <summary>
    /// [public] hp에 따라 활성화 / 비활성화되는 버프 관리
    /// </summary>
    /// <param name="hpRatio"></param>
    public void OnHpChanged(float hpRatio)
    {
        foreach (BuffInstance instance in _active)
        {
            if (instance.Source is not IHpRatioReactiveBuff reactive) { continue; }

            if (reactive.ShouldBeActive(hpRatio))
            {
                instance.Activate(_condition);
            }
            else
            {
                instance.Deactive(_condition);
            }
        }
    }

    /// <summary>
    /// [public] 피격 시 해제하는 버프일 경우 해제하기
    /// </summary>
    public void OnPlayerHit()
    {
        for (int i = _active.Count - 1; i >= 0; i--)
        {
            if (_active[i].ShouldRemoveOnHit())
            {
                Remove(_active[i]);
            }
        }
    }
    #endregion
}
