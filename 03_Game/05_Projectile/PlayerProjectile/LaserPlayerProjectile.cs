using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPlayerProjectile : PlayerProjectile
{

    int _seqCount = 5;

    public override void Spawn(Vector2 spawnPos, Vector2 dir)
    {

        this.transform.position = spawnPos + dir;

        dir.x *= -1f;
        dir.y *= -1f;

        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < _seqCount; i++)
        {
            Vector3 target = (Vector3)spawnPos + (Vector3)dir;

            seq.Append(transform.DOMove(target, data.AliveTime / _seqCount).SetEase(Ease.Linear));
            dir.x *= -1f;
            dir.y += dir.y;
        }

    }
}
