using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BoomerangPlayerProjectile : PlayerProjectile
{
    public override void Spawn(Vector2 spawnPos, Vector2 dir)
    {
        base.Spawn(spawnPos, dir);


        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        Vector3 center = PlayerManager.Instance.StagePlayer.transform.position;
        float minX = center.x - camWidth;
        float maxX = center.x + camWidth;
        float minY = center.y - camHeight;
        float maxY = center.y + camHeight;


        float absX = Mathf.Abs(dir.x);
        float absY = Mathf.Abs(dir.y);

        float maxDistX = camWidth;
        if (absX > 0.01f)
        {
            float endX = (dir.x > 0f) ? (maxX - spawnPos.x) : (spawnPos.x - minX);
            maxDistX = endX / absX;
        }

        float maxDistY = camHeight;
        if (absY > 0.01f)
        {
            float endY = (dir.y > 0f) ? (maxY - spawnPos.y) : (spawnPos.y - minY);
            maxDistY = endY / absY;
        }

        float maxDist = Mathf.Min(maxDistX, maxDistY);

        Vector3 targetPos = (Vector3)spawnPos + (Vector3)(dir * maxDist);
        Vector3 turnTargetPos = (Vector3)spawnPos - (Vector3)(dir * maxDist);


        Sequence seq = DOTween.Sequence();
        seq.Append(this.transform.DOMove(targetPos, data.AliveTime * 0.35f).SetEase(Ease.OutQuad));

        seq.Append(this.transform.DOMove(turnTargetPos, data.AliveTime * 0.65f).SetEase(Ease.InQuad));

        float spinPerSec = 720f;
        transform.rotation = Quaternion.identity;
        seq.Insert(0f,
            transform.DORotate(new Vector3(0f, 0f, spinPerSec * data.AliveTime), data.AliveTime, RotateMode.FastBeyond360).SetEase(Ease.Linear));

    }


}
