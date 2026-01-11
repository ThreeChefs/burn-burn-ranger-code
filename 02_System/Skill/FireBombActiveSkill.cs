using System.Collections;
using UnityEngine;

public class FireBombActiveSkill : ActiveSkill
{
    // 초토화때 사용
    //static float fireDelay = 0.1f;
    //WaitForSeconds fireDelayWait = new WaitForSeconds(fireDelay);

    float radius = 3f;

    protected override IEnumerator UseSkill(Transform target)
    {
        for (int i = 0; i < activeSkillData.ProjectilesCounts[CurLevel - 1]; ++i)
        {
            float angle = (360f / activeSkillData.ProjectilesCounts[CurLevel - 1]) * i;

            Vector3 anglePos = Quaternion.AngleAxis(angle, Vector3.forward) * (Vector3.right * radius);
            Vector3 firePos = this.transform.position + anglePos;

            ProjectileManager.Instance.Spawn(
                projectileIndex,
                PlayerManager.Instance.Condition[StatType.Attack],
                target,
                activeSkillData,
                firePos);

            yield return null ;
        }
    }



}
