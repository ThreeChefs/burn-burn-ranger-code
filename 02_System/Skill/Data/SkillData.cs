using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
    [field: Header("공통")]
    [field: NonSerialized] public int RuntimeIndex { get; set; }
    [field: Tooltip("스킬 이름")]
    [field: SerializeField] public string DisplayName { get; protected set; }
    [field: Tooltip("스킬 설명")]
    [field: MultiLineProperty]
    [field: SerializeField] public string[] Descriptions { get; protected set; }
    [field: Tooltip("스킬 이미지")]
    [field: SerializeField] public Sprite Icon { get; protected set; }
    [field: Tooltip("해금 스테이지")]
    [field: SerializeField] public int UnlockStageId { get; protected set; }

    [field: Tooltip("스킬 타입 (액티브 / 패시브)")]
    [field: SerializeField] public SkillType Type { get; protected set; }
    [field: Tooltip("돌파 조합 스킬 정보")]
    [field: SerializeField] public SkillData[] CombinationSkills { get; protected set; }
    [field: Tooltip("레벨에 따른 수치")]
    [field: SerializeField] public List<SkillLevelValueEntry> LevelValues { get; protected set; }

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        CombinationSkills = new SkillData[1];
        LevelValues = new();
        Descriptions = new string[1];
    }
#endif
}

[System.Serializable]
public class SkillLevelValueEntry
{
    [field: SerializeField] public SkillValueType SkillValueType { get; private set; }
    [field: SerializeField] public float[] Values { get; private set; }

    public SkillLevelValueEntry()
    {
        Values = new float[Define.SkillMaxLevel];
    }

    public SkillLevelValueEntry(SkillValueType skillValueType, int count = Define.SkillMaxLevel)
    {
        SkillValueType = skillValueType;
        Values = new float[count];
    }
}