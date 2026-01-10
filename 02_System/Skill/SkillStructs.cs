using UnityEngine;

public struct DamageEffect
{
    public float damage;
    public float tickInterval;
    public float duration;
    public Vector2 direction;
    public float knockbackForce;
}

/// <summary>
/// 피격 시 필요한 정보를 담은 struct
/// 읽기 전용 참조용
/// </summary>
public struct HitContext
{
    public float damage;
    public Transform attacker;
    public Vector2 position;
    public Collider2D directTarget;
    public ProjectileData projectileData;
}