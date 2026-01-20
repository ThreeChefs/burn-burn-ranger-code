using UnityEngine;

public class GoldItem : MonoBehaviour
{
    [SerializeField] private int goldAmount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out StagePlayer player))
            return;

        player.AddGold(goldAmount);
        SoundManager.Instance.PlaySfx(SfxName.Sfx_Item, idx: 2);

        Destroy(gameObject);
    }
}
