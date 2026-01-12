
using System.Collections;
using UnityEngine;

public class BrickActiveSkill : ActiveSkill
{

    protected override IEnumerator UseSkill(Transform target)
    {
        for (int i = 0; i < skillValues[SkillValueType.ProjectileCount][CurLevel - 1]; i++)
        {
            float randomDir = Random.Range(0.05f, 0.2f);
            ProjectileManager.Instance.Spawn(projectileIndex, this, new Vector2(randomDir,1f), transform.position);

            yield return null;
        }
    }
}
