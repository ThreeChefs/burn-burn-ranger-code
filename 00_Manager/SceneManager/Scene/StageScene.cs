using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScene : BaseScene
{
    private void Start()
    {
        UIManager.Instance.LoadUI(UIName.UI_Stage);
        
        // todo : 임시! 플레이어, 스테이지 정보 읽고 사용될 수 있는 것들을 쭉 등록해줘야함.
        ProjectileManager.Instance.UsePool(ProjectileDataIndex.KunaiProjectileData);
        ProjectileManager.Instance.UsePool(ProjectileDataIndex.RangedAttack);
        
        CommonPoolManager.Instance.UsePool(CommonPoolIndex.DamageText);
    }
}
