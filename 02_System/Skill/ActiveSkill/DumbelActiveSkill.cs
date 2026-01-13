using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbelActiveSkill : ActiveSkill
{

    protected override IEnumerator UseSkill(Transform target)
    {
        int count = (int)skillValues[SkillValueType.ProjectileCount][CurLevel - 1];

        for (int i = 0; i < skillValues[SkillValueType.ProjectileCount][CurLevel - 1]; ++i)
        {
            float angle = 360f / count * i;
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

            ProjectileManager.Instance.Spawn(projectileIndex, this, dir, this.transform.position);
        }

        // 한번에 스폰
        yield return null;
    }
}
