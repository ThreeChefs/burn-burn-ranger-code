using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronPlayerProjectile : PlayerProjectile
{
    public override void Spawn(Vector2 spawnPos, Vector2 dir)
    {
        //SetScale();

    }   

    public void SetTargetPosition(Vector2 targetPos)
    {
        this.transform.rotation = Quaternion.identity;
        Vector3 startPos = transform.position;

        // 랜덤 곡선 이동

        Vector3 middlePos = Vector3.Lerp(startPos, targetPos, 0.5f);

        middlePos = middlePos + new Vector3(Define.RandomRange(-2f, 2f), Define.RandomRange(1f, 3f));

        Vector3[] path = new Vector3[]
        {
            startPos, middlePos,targetPos
        };

        transform.DOPath(path,data.AliveTime, PathType.CatmullRom).SetEase(Ease.InOutSine);

    }


    Vector3 _prevPos;
    Quaternion _targetRot;

    private void Update()
    {
        base.Update();
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, _targetRot, Time.deltaTime * 10f);
    }


    private void FixedUpdate()
    {
        Vector3 currentPos = transform.position;
        Vector3 moveDir = currentPos - _prevPos;

        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.right, moveDir.normalized);
            _targetRot = rot;
        }

        _prevPos = currentPos;
    }



    protected override void OnDisableInternal()
    {
        base.OnDisableInternal();
        DOTween.Kill(transform);
    }

}
