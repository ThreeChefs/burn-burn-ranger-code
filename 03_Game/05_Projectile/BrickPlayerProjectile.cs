using DG.Tweening;
using UnityEngine;

public class BrickPlayerProjectile : PlayerProjectile
{
    Vector2 _dir;

    public override void Spawn(Vector2 spawnPos, Vector2 dir)
    {
        base.Spawn(spawnPos, dir);


        //test
        transform.DOMove(this.transform.position + Vector3.up * 10, 1f);
    }
    


    private void FixedUpdate()
    {
            
    }

}
