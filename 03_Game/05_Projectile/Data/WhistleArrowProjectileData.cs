using UnityEngine;

[CreateAssetMenu(fileName = "New ProjectileData", menuName = "SO/Projectile/Data - Whistle Arrow")]
public class WhistleArrowProjectileData : ProjectileData
{
    [field: Header("휘파람 화살 전용")]
    [field: SerializeField] public float RotationDuration { get; private set; }
    [field: SerializeField] public float FindTargetInterval { get; private set; }

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();

        RotationDuration = 0.2f;
        FindTargetInterval = 0.5f;
    }
#endif
}
