using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "SO/Skill")]
public class SkillData : ScriptableObject
{
    public int Id { get; private set; }                             // 아이디
    public string Name { get; private set; }                        // 이름
    public string Description { get; private set; }                 // 설명
    public SkillType Type { get; private set; }                     // 스킬 타입
    public List<int> Combinations { get; private set; }             // 돌파 조합 스킬 아이디

    // 액티브
    public List<float> LevelValueList { get; private set; }         // 레벨에 따른 수치값
    public List<float> ProjectilesCountList { get; private set; }   // 발사체 개수

    // 패시브
    public StatType StatType { get; private set; }                  // 타겟 스텟
}