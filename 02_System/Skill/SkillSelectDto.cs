using UnityEngine;

/// <summary>
/// 스킬 선택 시 보여줄 데이터 Dto
/// </summary>
public sealed class SkillSelectDto
{
    public int Id { get; }
    public int CurLevel { get; }
    public string Name { get; }
    public string Description { get; }
    public Sprite Icon { get; }
    public SkillType Type { get; }
    public Sprite[] CombinationIcons { get; }

    public SkillSelectDto(
        int id,
        int curLevel,
        string name,
        string description,
        Sprite icon,
        SkillType type,
        Sprite[] combinationIcons)
    {
        Id = id;
        CurLevel = curLevel;
        Name = name;
        Description = description;
        Icon = icon;
        Type = type;
        CombinationIcons = combinationIcons;
    }
}
