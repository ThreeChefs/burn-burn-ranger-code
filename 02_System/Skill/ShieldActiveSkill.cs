using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldActiveSkill : ActiveSkill
{
    PlayerProjectile _shieldProjectile;
    BaseStat _attack;
   
    public override void Init(SkillData data)
    {
        base.Init(data);
        this.transform.position = PlayerManager.Instance.StagePlayer.transform.position;
        _attack = PlayerManager.Instance.Condition[StatType.Attack];
        
        SetShield();
    }

    protected override IEnumerator UseSkill(Transform target)
    {
        // 아무것도 안함
        yield return null;
    }

    protected override void Update()
    {
        base.Update();

        if (_shieldProjectile != null)
        {
            _shieldProjectile.transform.position = this.transform.position;
        }

    }

    public override void LevelUp()
    {
        base.LevelUp();
        
        if (activeSkillData != null)
        {
            // Init 할 때 모든 Init 이 끝나고가 아니라 LevelUp 먼저 들어와서 처음에 여기서 data를 접근하면 없음...!
            SetShield();
        }

    }


    void SetShield()
    {
        if (_shieldProjectile != null)
            _shieldProjectile.gameObject.SetActive(false);

        _shieldProjectile = (PlayerProjectile)ProjectileManager.Instance.Spawn(ProjectileDataIndex.ShieldProjectileData, this, this.transform);
        //if(skillValues.ContainsKey(SkillValueType.Scale) == false) return;
        //_shieldProjectile.transform.localScale = Vector3.one * 2f * skillValues[SkillValueType.Scale][CurLevel - 1];
    }

    protected override void OnDestroy()
    {
        _shieldProjectile.gameObject?.SetActive(false);
        base.OnDestroy();
    }

}
