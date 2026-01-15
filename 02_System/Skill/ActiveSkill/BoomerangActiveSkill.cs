using System.Collections;
using UnityEngine;

public class BoomerangActiveSkill : ActiveSkill
{
    protected override IEnumerator UseSkill(Transform target = null)
    {
        for (int i = 0; i < SkillValues[SkillValueType.ProjectileCount][CurLevel-1];++i)
        {
            Vector3 randomDir = Random.insideUnitCircle.normalized;

            ProjectileManager.Instance.Spawn(projectileIndex, this, randomDir, this.transform.position);

            yield return projectileSpawnInterval;
        }
    }
}
