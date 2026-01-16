using UnityEngine;

public class PoisonMonster : Monster
{
    [SerializeField] private GameObject poisonAreaPrefab;

    public override void Die()
    {
        SpawnPoisonArea();
        base.Die();
    }

    private void SpawnPoisonArea()
    {
        if (poisonAreaPrefab == null)
        {
            return;
        }
        Instantiate(poisonAreaPrefab, transform.position, Quaternion.identity);
    }

}
