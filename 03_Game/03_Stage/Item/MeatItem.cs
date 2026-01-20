using UnityEngine;

public class MeatItem : MonoBehaviour
{
    [SerializeField] private float healAmount = 20f; // 회복량

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out StagePlayer player))
            return;


        PlayerStat health = player.Condition[StatType.Health];
        if (health != null)
        {
            health.Add(healAmount);
            Logger.Log($"고기 냠냠 HP → {health.CurValue}/{health.MaxValue}");
        }

        SoundManager.Instance.PlaySfx(SfxName.Sfx_Item, idx: 2);

        Destroy(gameObject);
    }
}
