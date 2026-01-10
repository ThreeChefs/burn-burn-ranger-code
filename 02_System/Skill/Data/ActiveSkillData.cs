using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillData", menuName = "SO/Skill/Active")]
public class ActiveSkillData : SkillData
{
    [field: Header("액티브 스킬 정보")]
    [field: Tooltip("쿨타임")]
    [field: SerializeField] public float Cooldown { get; private set; }
    [field: Tooltip("탄환 개수(-1: 연발)")]
    [field: SerializeField] public int[] ProjectilesCounts { get; private set; } = new int[Define.SkillMaxLevel];
    [field: Tooltip("탄환 오브젝트")]
    [field: SerializeField] public ProjectileData ProjectileData { get; private set; }
    [field: Tooltip("스킬 효과")]
    [field: SerializeField] public SkillConfig SkillConfig { get; private set; }

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        Type = SkillType.Active;
        LevelValue = new float[Define.SkillMaxLevel];
        ProjectileData = AssetLoader.FindAndLoadByName<ProjectileData>("KunaiProjectileData");
        SkillConfig = AssetLoader.FindAndLoadByName<SkillConfig>("DefaultSkillConfig");
    }
#endif
}
