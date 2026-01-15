using UnityEngine;

/// <summary>
/// 스킬 - 가시창
/// </summary>
public class ThornSpearActiveSkill : ActiveSkill
{
    private float _startAngleDeg;
    private float _angleStep;
    private int _index;
    private int _count;

    private Transform _origin;

    public override void Init(SkillData data)
    {
        base.Init(data);

        _startAngleDeg = 0f;
        _count = (int)skillValues[SkillValueType.ProjectileCount][CurLevel - 1];
        _index = 0;
        _angleStep = 360f / _count;
    }


    private void Start()
    {
        _origin = ProjectileManager.Instance.Spawn(
            ProjectileDataIndex.ThornSpearProjectileData,
            this,
            null,
            new Vector2(Random.Range(-1, 1), Random.Range(-1, 1))).transform;
    }

    protected override PlayerProjectile SpawnProjectile()
    {
        float angle = _startAngleDeg - _angleStep * _index;
        float rad = angle * Mathf.Deg2Rad;

        _index %= _count;

        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        return ProjectileManager.Instance.Spawn(projectileIndex, this, dir, _origin.position);
    }
}
