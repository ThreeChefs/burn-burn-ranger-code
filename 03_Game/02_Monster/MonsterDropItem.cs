using System.Collections.Generic;
using UnityEngine;
public class MonsterDropItem : MonoBehaviour
{
    public static MonsterDropItem Instance;
    [System.Serializable]

    public class DropPrefab
    {
        public DropItemType DropType;
        public GameObject prefab;
    }

    [SerializeField] private List<DropPrefab> dropPrefabs;
    private Dictionary<DropItemType, GameObject> _prefabMap;
    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


        _prefabMap = new Dictionary<DropItemType, GameObject>();

        foreach (var p in dropPrefabs)
        {
            _prefabMap[p.DropType] = p.prefab;
        }
    }


    public void Spawn(DropItemType type, Vector3 position)
    {
        if (TrySpawnGem(type, position))
            return;
        if (!_prefabMap.TryGetValue(type, out var prefab) || prefab == null)
        {
            Debug.LogError($"Drop prefab not found: {type}");
            return;
        }

        Instantiate(prefab, position, Quaternion.identity);
    }

    private bool TrySpawnGem(DropItemType type, Vector3 position)
    {
        // 여기 DropItemType 이름은 너 프로젝트에 맞춰 바꿔줘!
        // (예: DropItemType.BlueGem 이런 식으로 존재해야 함)

        switch (type)
        {
            case DropItemType.BlueGem:
                GemManager.Instance.SpawnGem(GemPoolIndex.BlueGem, position);
                return true;

            case DropItemType.GreenGem:
                GemManager.Instance.SpawnGem(GemPoolIndex.GreenGem, position);
                return true;

            case DropItemType.PurpleGem:
                GemManager.Instance.SpawnGem(GemPoolIndex.PurpleGem, position);
                return true;
        }

        return false;
    }
}

