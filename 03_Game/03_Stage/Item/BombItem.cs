using UnityEngine;

public class BombItem : MonoBehaviour
{

    [SerializeField] private bool destroyOnPickup = true;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<StagePlayer>(out var player))
            return;

        MonsterManager.Instance.KillAll();

        SoundManager.Instance.PlaySfx(SfxName.Sfx_Item, idx: 0);

        // 2) 폭탄 아이템 제거
        Destroy(gameObject);
    }


}

