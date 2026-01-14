using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 무한 맵
/// </summary>
public class InfiniteMap : MonoBehaviour
{
    [Header("맵")]
    [SerializeField] private Tilemap[] _tilemaps;
    [SerializeField] private Tilemap _defaultTilemapPrefab;

    private void Start()
    {
        // todo: 추후 스테이지 매니저에서 맵 불러올 때 처리
        if (_tilemaps[0] == null)
        {
            Init(_defaultTilemapPrefab);
        }
    }

    /// <summary>
    /// [public] 타일맵 배치
    /// </summary>
    /// <param name="tilemap"></param>
    public void Init(Tilemap tilemap)
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
        _defaultTilemapPrefab = AssetLoader.FindAndLoadByName("Tilemap_Default").GetComponent<Tilemap>();
    }
#endif
}
