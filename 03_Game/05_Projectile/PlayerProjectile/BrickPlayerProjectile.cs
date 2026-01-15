using DG.Tweening;
using UnityEngine;

public class BrickPlayerProjectile : PlayerProjectile
{
    static float _height = 8f;

    public override void Spawn(Vector2 spawnPos, Vector2 dir)
    {
        base.Spawn(spawnPos, dir);


        this.transform.rotation = Quaternion.identity;
        Vector3 startPos = transform.position;

        Sequence seq = DOTween.Sequence();

        seq.Append(
            transform.DOMove(startPos + (Vector3)(dir * _height), data.AliveTime * 0.35f).SetEase(Ease.OutQuad)
        );

        dir.y *= -1f;
        dir.x *= 2f;

        seq.Append(
            transform.DOMove(startPos + (Vector3)(dir * _height), data.AliveTime * 0.65f).SetEase(Ease.InQuad)
        );


    }


    Vector3 _prevPos;
    Quaternion _targetRot;

    protected override void Update()
    {
        base.Update();

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, _targetRot, Time.deltaTime * 10f);

    }


    protected override void FixedUpdate()
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
