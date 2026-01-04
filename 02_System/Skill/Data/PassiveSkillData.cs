using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkillData", menuName = "SO/Skill/Passive")]
public class PassiveSkillData : SkillData
{
    [field: Header("패시브 스킬 정보")]
    [field: SerializeField] public StatType StatType { get; private set; }                  // 타겟 스텟
}
