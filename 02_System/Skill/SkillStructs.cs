using UnityEngine;

/// <summary>
/// 피격 시 필요한 정보를 담은 struct
/// 읽기 전용 참조용
/// </summary>
public struct HitContext
{
    public float damage;
    public Transform attacker;              // 넉백에 필요해서
    public Vector2 hitPos;                  // 투사체의 이펙트가 일어나는 위치
    public Collider2D directTarget;         // 단일 타겟 피격 시 
    public ProjectileData projectileData;
}