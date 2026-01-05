using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillData", menuName = "SO/Skill/Active")]
public class ActiveSkillData : SkillData
{
    [field: Header("액티브 스킬 정보")]
    [field: Tooltip("쿨타임")]
    [field: SerializeField] public float Cooldown { get; private set; }
    [field: Tooltip("탄환 종류")]
    [field: SerializeField] public ProjectileType ProjectileType { get; private set; }
    [field: Tooltip("관통 횟수(-1: 무제한 / 0: 아님 / n: 횟수")]
    [field: SerializeField] public int PassCount { get; private set; }
    [field: Tooltip("탄환 개수(-1: 연발)")]
    [field: SerializeField] public int[] ProjectilesCounts { get; private set; } = new int[Define.SkillMaxLevel];
    [field: Tooltip("탄환 오브젝트")]
    [field: SerializeField] public GameObject Projectile { get; private set; }
    [field: Tooltip("탄환 생성 오프셋")]
    [field: SerializeField] public Vector2 Offset { get; private set; }
    [field: Tooltip("탄환 스피드")]
    [field: SerializeField] public float Speed { get; private set; }
}
