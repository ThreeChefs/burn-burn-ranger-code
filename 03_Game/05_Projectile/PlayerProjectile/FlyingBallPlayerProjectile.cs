using UnityEngine;

public class FlyingBallPlayerProjectile : PlayerProjectile
{
    

    [SerializeField] float _radiusExpandSpeed = 2f;
    [SerializeField] float _angleExpandSpeed = 360;

    Vector3 _spawnDir = Vector3.zero;
    Vector3 _startPos;
    float _nowAngle = 0f;
    float _nowRadius = 0;


    public override void Spawn(Vector2 spawnPos, Vector2 dir)
    {
        base.Spawn(spawnPos, dir);

        _startPos = spawnPos;
        _nowAngle = 0;
        _nowRadius = 0;
        _spawnDir = dir;

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

        _nowRadius += _radiusExpandSpeed * Time.deltaTime;
        _nowAngle += _angleExpandSpeed * Time.deltaTime;
        Vector3 dir = Quaternion.AngleAxis(_nowAngle, Vector3.forward) * _spawnDir;
        transform.position = _startPos + (dir * _nowRadius);


        // 회전
        Vector3 moveDir = currentPos - _prevPos;
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.right, moveDir.normalized);
            _targetRot = rot;
        }

        _prevPos = currentPos;
    }


}