using UnityEngine;

public class BossTiles : BaseProjectile
{
    private float damage;

    [SerializeField] private float lifeTime = 3f;
    private float _disableTile;
    public void Init(float damage, float lifeTime)
    {
        this.damage = damage;
        this.lifeTime = lifeTime;
    }

    protected override void OnEnableInternal()
    {
        base.OnEnableInternal();
        _disableTile = Time.time + lifeTime;
    }





}
