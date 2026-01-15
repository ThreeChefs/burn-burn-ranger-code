using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActiveSkill : ActiveSkill
{

    float _laserDistance = 3f;

    float _moveHeight = 0.5f;
    float _moveWidth = 0.7f;

    protected override IEnumerator UseSkill(Transform target)
    {
        Vector3 spawnDir = Vector3.zero;

        // 4 방향 위치로 스폰
        spawnDir.x = _moveWidth;
        spawnDir.y = _moveHeight;
        ProjectileManager.Instance.Spawn(projectileIndex, this, spawnDir * _laserDistance, this.transform.position);

        spawnDir.x = _moveWidth;
        spawnDir.y = -_moveHeight;
        ProjectileManager.Instance.Spawn(projectileIndex, this, spawnDir * _laserDistance, this.transform.position);

        spawnDir.x = -_moveWidth;
        spawnDir.y = _moveHeight;
        ProjectileManager.Instance.Spawn(projectileIndex, this, spawnDir * _laserDistance, this.transform.position);

        spawnDir.x = -_moveWidth;
        spawnDir.y = -_moveHeight;
        ProjectileManager.Instance.Spawn(projectileIndex, this, spawnDir * _laserDistance, this.transform.position);




        yield return null;
    }
}
