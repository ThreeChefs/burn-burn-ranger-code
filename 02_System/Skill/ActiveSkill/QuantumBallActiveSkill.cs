using System.Collections;
using UnityEngine;

/// <summary>
/// 스킬 - 양자공
/// </summary>
public class QuantumBallActiveSkill : ActiveSkill
{
    private Transform prevProjectile;

    public override void Init(SkillData data)
    {
        base.Init(data);
        projectileSpawnInterval = new(projectileData.AliveTime);
    }

    protected override IEnumerator UseSkill(Transform target = null)
    {
        prevProjectile = ProjectileManager.Instance
            .Spawn(projectileIndex, this, transform.forward)
            .GetComponent<Transform>();

        Logger.Log("대기");
        yield return projectileSpawnInterval;

        int count = (int)skillValues[SkillValueType.ProjectileCount][CurLevel - 1];
        int rand = Random.Range(count / 2, count);
        for (int i = 0; i < rand; i++)
        {
            Vector2 dir = Quaternion.Euler(0, 0, Random.Range(0f, 360f)) * prevProjectile.forward;
            ProjectileManager.Instance.Spawn(projectileIndex, this, dir, prevProjectile.position);
        }
    }
}
