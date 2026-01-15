using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FireBombActiveSkill : ActiveSkill
{
    // 초토화때 사용
    [SerializeField] List<Transform> _visualSprs;

    float radius = 4f;

    protected override IEnumerator UseSkill(Transform target)
    {
        int fireCount = 0;

        if (skillValues.ContainsKey(SkillValueType.ProjectileCount))
        {
            for (int i = 0; i < skillValues[SkillValueType.ProjectileCount][CurLevel - 1]; ++i)
            {
                float angle = (360f / skillValues[SkillValueType.ProjectileCount][CurLevel - 1]) * i;

                Vector3 anglePos = Quaternion.AngleAxis(angle, Vector3.forward) * (Vector3.right * radius);
                Vector3 firePos = PlayerManager.Instance.StagePlayer.transform.position + anglePos;

                if (SkillData.Type == SkillType.Combination)
                {
                    ThrowFireBomb(fireCount, target, firePos);
                    fireCount++;
                    yield return projectileSpawnInterval;
                }
                else
                {
                    ThrowFireBomb(fireCount, target, firePos);
                    fireCount++;
                }
            }

        }

    }


    void ThrowFireBomb(int count, Transform target, Vector3 firePos)
    {
        if (_visualSprs.Count <= count)
        {
            SpawnFireBombProjectile(firePos, target);
        }
        else
        {
            _visualSprs[count].gameObject.SetActive(true);
            _visualSprs[count].transform.position = this.transform.position;

            Sequence _flySeq = DOTween.Sequence();

            _flySeq.Append(_visualSprs[count].DOMove(firePos, 0.5f).SetEase(Ease.Linear));
            _flySeq.Join(_visualSprs[count].DORotate(new Vector3(0, 0, 120), 0.5f, RotateMode.WorldAxisAdd));
            _flySeq.OnComplete(() =>
            {
                SpawnFireBombProjectile(firePos, target, count);

            });    // 매번 캡쳐 해결 필요

        }

    }

    void SpawnFireBombProjectile(Vector3 firePos, Transform target)
    {
        ProjectileManager.Instance.Spawn(projectileIndex, this, target, firePos);
    }


    void SpawnFireBombProjectile(Vector3 firePos, Transform target, int count)
    {
        _visualSprs[count].gameObject.SetActive(false);
        ProjectileManager.Instance.Spawn(projectileIndex, this, target, firePos);
    }



}
