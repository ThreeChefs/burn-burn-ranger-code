using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ProjectileData", menuName = "SO/Projectile/Data")]
public class ProjectileData : PoolObjectData
{
    [field: Title("생명 주기")]
    [field: Tooltip("생존 시간")]
    [field: SerializeField] public float AliveTime { get; private set; }
    [field: Tooltip("관통 횟수(-100: 무제한 / 1 ~ n: 횟수")]
    [field: SerializeField] public int PassCount { get; private set; }

    [field: Title("이동")]
    [field: Tooltip("탄환 스피드")]
    [field: SerializeField] public float Speed { get; private set; }
    [field: Tooltip("탄환 이동 - 기본")]
    [field: SerializeField] public ProjectileBaseMoveType BaseMoveType { get; private set; }
    [field: ShowIf(nameof(BaseMoveType), ProjectileBaseMoveType.Straight)]
    [field: Tooltip("탄환 이동 - 특성")]
    [field: SerializeField] public ProjectileMoveFeature MoveFeature { get; private set; }


    [field: ShowIf("@MoveFeature.HasFlag(ProjectileMoveFeature.Guidance)")]
    [field: Tooltip("유도 성능(유도 시간")]
    [field: SerializeField] public float GuidanceTime { get; private set; }

    [field: ShowIf("@MoveFeature.HasFlag(ProjectileMoveFeature.Reflection)")]
    [field: Tooltip("반사 타겟 레이어")]
    [field: SerializeField] public LayerMask ReflectionLayerMask { get; private set; }

    [field: Header("공격")]
    [field: Tooltip("히트 모드(즉발 / 유지 / 특정 시점)")]
    [field: SerializeField] public ProjectileHitType HitType { get; private set; }
    [field: Tooltip("타겟 레이어")]
    [field: SerializeField] public LayerMask TargetLayerMask { get; private set; }
    [field: Tooltip("넉백")]
    [field: SerializeField] public float KnockBack { get; private set; }
    [field: SerializeField] public List<BaseSkillEffectSO> HitEffects { get; private set; }

    [field: Header("비주얼")]
    [field: Tooltip("비주얼 (2D)")]
    [field: SerializeField] public ProjectileVisualData VisualData { get; private set; }


    [field: Header("폭발 / 장판 (2D)")]
    [field: SerializeField] public bool HasAreaPhase { get; private set; }
    [field: ShowIf(nameof(HasAreaPhase))]
    [field: SerializeField] public AoEData AoEData { get; private set; }

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        AliveTime = 2f;
    }
#endif
}

[System.Serializable]
public class AoEData
{
    [field: Tooltip("Fly 상태 유지 시간 (-1: 피격 때까지 날아감")]
    [field: SerializeField] public float FlyPhaseDuration { get; private set; }
    [field: Tooltip("즉발기")]
    [field: SerializeField] public bool IsInstant { get; private set; }
    [field: Tooltip("장판 움직임")]
    [field: SerializeField] public bool IsMoving { get; private set; }
    [field: HideIf(nameof(IsInstant))]
    [field: Tooltip("장판 수명")]
    [field: SerializeField] public float Duration { get; private set; }
    [field: HideIf(nameof(IsInstant))]
    [field: SerializeField] public float TickInterval { get; private set; }
    [field: Tooltip("장판 모양")]
    [field: SerializeField] public AoEShape AoEShape { get; private set; }

    [field: ShowIf(nameof(AoEShape), AoEShape.Circle)]
    [field: Tooltip("폭발 반경")]
    [field: SerializeField] public float Radius { get; private set; }

    [field: ShowIf(nameof(AoEShape), AoEShape.Box)]
    [field: Tooltip("박스 크기")]
    [field: SerializeField] public Vector2 BoxSize { get; private set; }

    [field: SerializeField] public LayerMask AoETargetLayer { get; private set; }
    [field: HideIf(nameof(IsInstant))]
    [field: SerializeField] public ProjectileDataIndex Index { get; private set; }
    [field: SerializeField] public List<BaseSkillEffectSO> AreaEffects { get; private set; }
}