using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldActiveSkill : ActiveSkill
{
    //[SerializeField] BaseProjectile _projectile;
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] Transform _shieldTransform;

    float _tickDelay = 0.1f;
    float _nowTick = 0f;

    BaseStat _attack;
   
    public override void Init(SkillData data)
    {
        base.Init(data);
        this.transform.position = PlayerManager.Instance.StagePlayer.transform.position;
        _shieldTransform.transform.localScale = Vector3.one * SkillData.LevelValue[CurLevel - 1];
        _attack = PlayerManager.Instance.Condition[StatType.Attack];
        // 가지고 있을 프로젝타일? 한테 넣어주기
    }

    protected override IEnumerator UseSkill(Transform target)
    {
        // 아무것도 안함
        yield return null;
    }

    private void Update()
    {
        _nowTick += Time.deltaTime;

        if(_nowTick >= _tickDelay)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, SkillData.LevelValue[CurLevel - 1], Vector2.up, 0, _targetLayer);

            if (hits.Length > 0)
            {
                for(int i  = 0; i < hits.Length; ++i)
                {
                    if (hits[i].collider.TryGetComponent<IDamageable>(out var damageable))
                    {
                        damageable.TakeDamage(SkillData.LevelValue[CurLevel-1] * _attack.CurValue);
                    }
                }
            }

            _nowTick = 0;
        }
    }

    public override void LevelUp()
    {
        base.LevelUp();
        // 레벨업 된 수치 적용하기? 안해도되나

        _shieldTransform.transform.localScale = Vector3.one * SkillData.LevelValue[CurLevel-1];
    }

}
