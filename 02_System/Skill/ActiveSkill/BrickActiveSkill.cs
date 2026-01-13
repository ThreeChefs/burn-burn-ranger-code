
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class BrickActiveSkill : ActiveSkill
{
    readonly float _minX = -0.35f;
    readonly float _maxX = 0.35f;
    //readonly float _projectileFireInterval = 0.1f
    WaitForSeconds _projectileFireIntervalWait = new WaitForSeconds(0.1f);

    protected override IEnumerator UseSkill(Transform target)
    {
        for (int i = 0; i < skillValues[SkillValueType.ProjectileCount][CurLevel - 1]; i++)
        {
            float randomDir = Random.Range(_minX, _maxX);
            ProjectileManager.Instance.Spawn(projectileIndex, this, new Vector2(randomDir, 1f), transform.position);

            yield return _projectileFireIntervalWait;
        }
    }
}
