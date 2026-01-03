using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillData", menuName = "SO/Skill/Active")]
public class ActiveSkillData : SkillData
{
    [field: Header("액티브 스킬 정보")]
    [field: Tooltip("쿨타임")]
    [field: SerializeField] public float Cooldown { get; private set; }
    [field: Tooltip("탄환 개수")]
    [field: SerializeField] public int[] ProjectilesCounts { get; private set; } = new int[Define.SkillMaxLevel];
    [field: Tooltip("탄환 오브젝트")]
    [field: SerializeField] public GameObject Projectile { get; private set; }
}
