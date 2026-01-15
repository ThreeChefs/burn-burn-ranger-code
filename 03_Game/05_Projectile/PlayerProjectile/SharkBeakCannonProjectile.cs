using UnityEngine;

public class SharkBeakCannonProjectile : PlayerProjectile
{
    public override void Spawn(Vector2 spawnPos, Transform target)
    {
        base.Spawn(spawnPos, target);

        passCount = Random.Range(data.PassCount / 2, data.PassCount);
    }
}
