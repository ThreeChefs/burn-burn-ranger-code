using UnityEngine;

public class MagnetItem : MonoBehaviour
{

    [SerializeField] private bool pullGems = true;

    [SerializeField] private float destroyDelay = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<StagePlayer>(out var player))
            return;

        if (pullGems)
        {
            GemManager.Instance.MagnetGems(player.transform);
        }
        else
        {
            GemManager.Instance.CollectAllGemsInstant(player);
        }
        SoundManager.Instance.PlaySfx(SfxName.Sfx_Item, idx: 2);
        //  자석 아이템 제거
        if (destroyDelay <= 0f) Destroy(gameObject);
        else Destroy(gameObject, destroyDelay);
    }


}
