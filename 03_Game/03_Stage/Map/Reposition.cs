using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 맵 재배치
/// </summary>
[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapRenderer))]
public class Reposition : MonoBehaviour
{
    private Transform _player;
    private float _threshold;

    private const string SortingLayer = "BackGround";

    private void Awake()
    {
        TilemapRenderer render = GetComponent<TilemapRenderer>();
        render.sortingLayerName = SortingLayer;
    }

    private void Start()
    {
        _player = PlayerManager.Instance.StagePlayer.transform;
        _threshold = Define.MapSize * 1.05f;
    }

    private void Update()
    {
        Vector3 diff = _player.position - transform.position;

        if (Mathf.Abs(diff.x) > _threshold)
        {
            transform.position += Mathf.Sign(diff.x) * Define.MapSize * 2f * Vector3.right;
        }
        else if (Mathf.Abs(diff.y) > _threshold)
        {
            transform.position += Mathf.Sign(diff.y) * Define.MapSize * 2f * Vector3.up;
        }
    }
}
