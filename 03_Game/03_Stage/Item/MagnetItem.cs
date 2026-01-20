using UnityEngine;

public class MagnetItem : MonoBehaviour
{

    [SerializeField] private bool pullGems = true;

    [SerializeField] private float destroyDelay = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<StagePlayer>(out var player))
            return;

        //  자석 : 필드의 젬들을 처리
        ActivateMagnet(player);
        SoundManager.Instance.PlaySfx(SfxName.Sfx_Item, idx: 2);
        //  자석 아이템 제거
        if (destroyDelay <= 0f) Destroy(gameObject);
        else Destroy(gameObject, destroyDelay);
    }

    private void ActivateMagnet(StagePlayer player)
    {
        // 씬에 있는 모든 젬 찾기
        var gems = Object.FindObjectsByType<GemItem>(FindObjectsSortMode.None);

        if (pullGems)
        {
            // 젬이 플레이어로 빨려오게
            for (int i = 0; i < gems.Length; i++)
            {
                gems[i].StartMagnet(player.transform);
            }
        }
        else
        {
            // 즉시 경험치로 지급(연출 없이)
            for (int i = 0; i < gems.Length; i++)
            {
                player.StageLevel.AddExp(gems[i].ExpValue);
                Destroy(gems[i].gameObject);
            }
        }
    }
}
