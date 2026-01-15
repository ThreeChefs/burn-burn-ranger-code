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
        float rad = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

        _origin = ProjectileManager.Instance.Spawn(
            ProjectileDataIndex.ThornSpearProjectileData,
            this,
            dir,
            transform.position).transform;
    }

    protected override PlayerProjectile SpawnProjectile()
    {
        float angle = _startAngleDeg - _angleStep * _index;
        float rad = angle * Mathf.Deg2Rad;

        _index = (_index + 1) % _count;

        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        return ProjectileManager.Instance.Spawn(projectileIndex, this, dir, _origin.position);
    }
}
