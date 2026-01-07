using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New ProjectileData", menuName = "SO/Projectile")]
public class ProjectileData : PoolObjectData
{
    [field: Header("공통")]
    [field: Tooltip("공격력 계수")]
    [field: SerializeField] public float DamageMultiplier { get; private set; }
    [field: Tooltip("탄환 스피드")]
    [field: SerializeField] public float Speed { get; private set; }
    [field: Tooltip("탄환 타입")]
    [field: SerializeField] public ProjectileType ProjectileType { get; private set; }
    [field: Tooltip("관통 횟수(-1: 무제한 / 1 ~ n: 횟수")]
    [field: SerializeField] public int PassCount { get; private set; }
    [field: Tooltip("생존할 시간(-1: 충돌할 때까지 생존")]
    [field: SerializeField] public float AliveTime { get; private set; }
    [field: Tooltip("타겟 레이어")]
    [field: SerializeField] public LayerMask TargetLayerMask { get; private set; }

    // 유도
    [field: Header("유도")]
    [field: ShowIf("ProjectileType", ProjectileType.Guidance)]
    [field: Tooltip("유도 성능(유도 시간")]
    [field: SerializeField] public float GuidanceTime { get; private set; }

    [field: Header("반사")]
    [field: ShowIf("ProjectileType", ProjectileType.Reflection)]
    [field: Tooltip("반사 타겟 레이어")]
    [field: SerializeField] public LayerMask ReflectionLayerMask { get; private set; }

#if UNITY_EDITOR
    private void Reset()
    {
        DamageMultiplier = 1f;
        AliveTime = 2f;
    }
#endif
}
