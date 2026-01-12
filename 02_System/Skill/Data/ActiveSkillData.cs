using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillData", menuName = "SO/Skill/Active")]
public class ActiveSkillData : SkillData
{
    [field: Header("액티브 스킬 정보")]
    [field: Tooltip("쿨타임")]
    [field: SerializeField] public float Cooldown { get; private set; }
    [field: Tooltip("탄환 소환 간격")]
    [field: SerializeField] public float SpawnInterval { get; private set; }
    [field: Tooltip("탄환 오브젝트")]
    [field: SerializeField] public ProjectileData ProjectileData { get; private set; }
    [field: Tooltip("액티브 스킬 프리팹")]
    [field: SerializeField] public ActiveSkill ActiveSkillPrefab { get; private set; }

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        Type = SkillType.Active;
        LevelValues.Add(new SkillLevelValueEntry(SkillValueType.AttackPower));
        LevelValues.Add(new SkillLevelValueEntry(SkillValueType.ProjectileCount));
        LevelValues.Add(new SkillLevelValueEntry(SkillValueType.Scale));
        ProjectileData = AssetLoader.FindAndLoadByName<ProjectileData>("KunaiProjectileData");
    }
#endif
}
