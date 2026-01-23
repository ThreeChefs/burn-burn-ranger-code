using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 무한 맵
/// </summary>
public class InfiniteMap : MonoBehaviour
{
    [Header("맵")]
    [SerializeField] private Tilemap _tilemapPrefab;
    [SerializeField] private Tilemap[] _tilemaps;

    private void Start()
    {
        SpawnTilemap(_tilemapPrefab);
    }

    /// <summary>
    /// 타일맵 배치
    /// </summary>
    /// <param name="tilemap"></param>
    private void SpawnTilemap(Tilemap tilemap)
    {
        for (int i = 0; i < Define.TilemapCount; i++)
        {
            Tilemap newTilemap = Instantiate(tilemap, transform);
            _tilemaps[i] = newTilemap;

            _tilemaps[i].transform.localPosition = new Vector2(
                Define.MapSize / 2 * (i % 2 == 0 ? -1 : 1),
                Define.MapSize / 2 * (i < 2 ? -1 : 1));
        }
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _tilemaps = new Tilemap[4];
        _tilemapPrefab = AssetLoader.FindAndLoadByName("Tilemap_Default").GetComponent<Tilemap>();
    }
#endif
}
