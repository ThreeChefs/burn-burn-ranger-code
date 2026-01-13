using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronActiveSkill : ActiveSkill
{
    enum DronType       // 내부에서만 쓸 enum
    {
        A,
        B,
        Destroyer,
    }

    [SerializeField] Dron _dronOrigin;
    [SerializeField] Transform _dronPivot;
    [SerializeField] DronType _type;

    Dron _dron;
    WaitForSeconds _wait = new WaitForSeconds(0.01f);
    float _fireDistance = 4f;
    float _randomRange = 0.5f;

    public override void Init(SkillData data)
    {
        base.Init(data);

        _dron = Instantiate(_dronOrigin);
        _dron.SetTarget(_dronPivot);

    }

    protected override IEnumerator UseSkill(Transform target)
    {
        int count = (int)skillValues[SkillValueType.ProjectileCount][CurLevel - 1];

        for (int i = 0; i < skillValues[SkillValueType.ProjectileCount][CurLevel - 1]; ++i)
        {
            float angle = 360f / count * i;
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

            Vector3 targetPos = this.transform.position + (dir * _fireDistance);
            
            Vector3 randomOffset =
                  new Vector3(
                    Define.RandomRange(-_randomRange, _randomRange),
                    Define.RandomRange(-_randomRange, _randomRange),
                    0f
                );

            Vector3 targetRandoPos = targetPos + randomOffset;

            DronPlayerProjectile dronProjectile = (DronPlayerProjectile)ProjectileManager.Instance.Spawn(projectileIndex, this, dir, _dron.transform.position);
            if (dronProjectile != null)
            {
                dronProjectile.SetTargetPosition(targetRandoPos);
            }

            CommonPoolManager.Instance.Spawn(CommonPoolIndex.DronAim, targetRandoPos);

            yield return _wait;

        }

    }


}
