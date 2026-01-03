using System.Collections.Generic;
using UnityEngine;

public class MonsterDropItem : MonoBehaviour
{
    public static MonsterDropItem Instance;

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

        // Inspector에서 등록한 List를
        // Dictionary로 변환 (빠른 조회용)
        _prefabMap = new Dictionary<DropItemType, GameObject>();

        foreach (var p in dropPrefabs)
        {
            // 같은 타입이 여러 번 들어오면 마지막 값으로 덮어씀
            _prefabMap[p.DropType] = p.prefab;
        }
    }


    public void Spawn(DropItemType type, Vector3 position)
    {
        // 해당 타입의 프리팹이 등록되어 있는지 확인
        if (!_prefabMap.TryGetValue(type, out var prefab))
        {
            Debug.LogError($"Drop prefab not found: {type}");
            return;
        }

        // 프리팹을 지정 위치에 생성
        Instantiate(prefab, position, Quaternion.identity);
    }
}
