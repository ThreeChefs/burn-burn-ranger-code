using System.Collections;
using UnityEngine;

public class GuardianActiveSkill : ActiveSkill
{
    [SerializeField] private float _multiplier = 1.5f;
    private int _count;
    private int _index;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override IEnumerator UseSkill(Transform target = null)
    {
        Vector2 originPos = transform.position;
        yield return projectileSpawnInterval;
        for (int i = 0; i < _count; i++)
        {
            ProjectileManager.Instance.Spawn(
                projectileIndex,
                this,
                Vector2.zero,
                position: CalcSpawnPos(originPos),
                parent: transform);
        }
    }

    private Vector2 CalcSpawnPos(Vector2 standardPos)
    {
        float rad = 360f / _count * _index * Mathf.Deg2Rad;
        _index = (_index + 1) % _count;

        Vector2 pos = standardPos + new Vector2(Mathf.Cos(rad) * _multiplier, Mathf.Sin(rad) * _multiplier);
        return pos;
    }

    public override void LevelUp()
    {
        base.LevelUp();
        _count = (int)skillValues[SkillValueType.ProjectileCount][CurLevel - 1];
        _animator.speed = skillValues[SkillValueType.ProjectileSpeed][CurLevel - 1];
    }
}