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
        if (poisonAreaPrefab == null) return;
        ProjectileManager.Instance.Spawn(ProjectileDataIndex.PoisonArea, Attack, Vector2.zero, transform.position, Quaternion.identity, parent: null);

    }


}
