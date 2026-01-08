using UnityEngine;

public class BombItem : MonoBehaviour
{

    [SerializeField] private bool destroyOnPickup = true;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<StagePlayer>(out var player))
            return;

        // 1) 폭탄 효과 발동
        KillAllMonsters();

        // 2) 폭탄 아이템 제거
        Destroy(gameObject);
    }

    private void KillAllMonsters()
    {
        // 씬에 존재하는 모든 Monster 컴포넌트 찾기 (비활성 오브젝트 제외)
        var monsters = FindObjectsOfType<Monster>();

        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i] == null) continue;
            monsters[i].BombDie(); // 내부에서 Die() 호출
        }
    }
}

