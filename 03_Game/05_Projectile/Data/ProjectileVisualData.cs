using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New ProjectileVisualData", menuName = "SO/Projectile/Visual Data")]
public class ProjectileVisualData : ScriptableObject
{
    [field: Header("이미지")]
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Color Color { get; private set; } = Color.white;
    [field: SerializeField] public Vector2 BaseScale { get; private set; } = Vector2.one;

    [field: Header("생명 주기")]
    [field: Tooltip("true면 Projectile 생존 시간과 동일, false면 VisualDuration 사용")]
    [field: SerializeField] public bool FollowProjectileLifetime { get; private set; } = true;
    [field: HideIf(nameof(FollowProjectileLifetime))]
    [field: SerializeField] public float VisualDuration { get; private set; }

    [field: Header("애니메이션")]
    [field: SerializeField] public RuntimeAnimatorController Animaton { get; private set; }

    [field: Header("VFX")]
    [field: SerializeField] public GameObject TrailVfxPrefab { get; private set; }
    [field: SerializeField] public GameObject HitVfxPrefab { get; private set; }
    [field: SerializeField] public GameObject ExplosionVfxPrefab { get; private set; }
}
