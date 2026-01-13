using System.Collections;
using UnityEngine;

public class ThunderActiveSkill : ActiveSkill
{
    protected override IEnumerator UseSkill(Transform target)
    {
        WaitForSeconds _projectileFireIntervalWait = new WaitForSeconds(Data.SpawnInterval);
        for (int i = 0; i < skillValues[SkillValueType.ProjectileCount][CurLevel - 1]; i++)
        {
            Transform monster = MonsterManager.Instance.GetRandomMonster();
            if (monster == null) break;
            ProjectileManager.Instance.Spawn(projectileIndex, this, Vector3.zero, monster.transform.position);

            yield return _projectileFireIntervalWait;
        }
    }
}
