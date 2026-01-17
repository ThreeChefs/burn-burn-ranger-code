using UnityEngine;

[CreateAssetMenu(fileName = "New ProjectileVisualData", menuName = "SO/Projectile/Visual Data")]
public class ProjectileVisualData : ScriptableObject
{
    [field: Header("효과음")]
    [field: Tooltip("Sfx_Projectile 오디오 클립 인덱스. 효과음 없게 하고 싶을 경우: -1")]
    [field: SerializeField] public int SfxIndex { get; private set; }
    [field: Tooltip("장판일 경우 효과음 간격")]
    [field: SerializeField] public float SfxInterval { get; private set; }

#if UNITY_EDITOR
    private void Reset()
    {
        SfxInterval = 1.5f;
    }
#endif
}
