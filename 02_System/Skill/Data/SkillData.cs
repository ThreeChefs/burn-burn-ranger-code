using UnityEngine;

public class SkillData : ScriptableObject
{
    [field: Header("공통")]
    [field: Tooltip("스킬 번호")]
    [field: SerializeField] public int Id { get; protected set; }
    [field: Tooltip("스킬 이름")]
    [field: SerializeField] public string Name { get; protected set; }
    [field: Tooltip("스킬 설명")]
    [field: SerializeField] public string Description { get; protected set; }
    [field: Tooltip("스킬 이미지")]
    [field: SerializeField] public Sprite Sprite { get; protected set; }
    [field: Tooltip("해금 스테이지")]
    [field: SerializeField] public int UnlockStageId { get; protected set; }

    [field: Tooltip("스킬 타입 (액티브 / 패시브)")]
    [field: SerializeField] public SkillType Type { get; protected set; }
    [field: Tooltip("돌파 조합 스킬 정보")]
    [field: SerializeField] public int[] CombinationIds { get; protected set; }
    [field: Tooltip("레벨에 따른 수치 (플레이어의 기본 공격력에 곱해짐)")]
    [field: SerializeField] public float[] LevelValue { get; protected set; } = new float[Define.SkillMaxLevel];
}