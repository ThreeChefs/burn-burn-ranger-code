using UnityEngine;

/// <summary>
/// 스킬 효과 기본 클래스
/// </summary>
public abstract class BaseEffectSO : ScriptableObject
{
    public abstract void Apply(in HitContext context);
}
