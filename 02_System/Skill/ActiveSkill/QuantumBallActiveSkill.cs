using System.Collections;
using UnityEngine;

/// <summary>
/// 스킬 - 양자공
/// </summary>
public class QuantumBallActiveSkill : ActiveSkill
{
    private Transform prevProjectile;

    protected override IEnumerator UseSkill(Transform target = null)
    {
        target = MonsterManager.Instance.GetNearestMonster();
        if (target != null)
        {
            prevProjectile = ProjectileManager.Instance
                .Spawn(projectileIndex, this, target, transform.position)
                .GetComponent<Transform>();

            Logger.Log("대기");
            yield return projectileSpawnInterval;

            int count = (int)skillValues[SkillValueType.ProjectileCount][CurLevel - 1];
            int rand = Random.Range(count / 2, count);
            for (int i = 0; i < rand; i++)
            {
                Vector3 dir = prevProjectile.forward + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                dir.Normalize();
                ProjectileManager.Instance.Spawn(projectileIndex, this, dir, prevProjectile.position);
            }
        }
    }
}
