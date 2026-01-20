using UnityEngine;

public class FortuneBox : PoolObject
{

    WaveClearRewardType _rewardType;

    public void Init(WaveClearRewardType rewardType)
    {
        _rewardType = rewardType;
    }


    void ShowFortuneBoxUI()
    {
        switch (_rewardType)
        {
            case WaveClearRewardType.Fortune_Skill_Random:
                Debug.Log("랜덤행운열차");
                break;
            case WaveClearRewardType.Fortune_Skill_1:
                Debug.Log("스킬 1개 획득");
                break;
            case WaveClearRewardType.Fortune_Skill_3:
                Debug.Log("스킬 3개 획득");
                break;
            case WaveClearRewardType.Fortune_Skill_5:
                Debug.Log("스킬 5개 획득");
                break;
        }

    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == Define.PlayerLayer)
        {
            ShowFortuneBoxUI();
            gameObject.SetActive(false);
        }
        SoundManager.Instance.PlaySfx(SfxName.Sfx_Item, idx: 2);

    }

}
