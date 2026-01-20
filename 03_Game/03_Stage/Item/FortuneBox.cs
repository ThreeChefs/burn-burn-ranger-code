using UnityEngine;

public class FortuneBox : PoolObject
{

    WaveClearRewardType _rewardType;

    public void Init(WaveClearRewardType rewardType)
    {
        _rewardType = rewardType;
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == Define.PlayerLayer)
        {
            StageManager.Instance?.ShowFortuneBoxUI(_rewardType);
            gameObject.SetActive(false);
        }
        SoundManager.Instance.PlaySfx(SfxName.Sfx_Item, idx: 2);

    }

}
