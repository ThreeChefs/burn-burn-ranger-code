using UnityEngine;

/// <summary>
/// 무한 맵
/// </summary>
public class InfiniteMap : MonoBehaviour
{
    [Header("맵")]
    [SerializeField] private SpriteRenderer[] _renderers;

    public void Init(Sprite sprite)
    {
        foreach (SpriteRenderer renderer in _renderers)
        {
            renderer.sprite = sprite;
        }
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _renderers = new SpriteRenderer[4];
        _renderers[0] = transform.FindChild<SpriteRenderer>("Square");
        _renderers[1] = transform.FindChild<SpriteRenderer>("Square_1");
        _renderers[2] = transform.FindChild<SpriteRenderer>("Square_2");
        _renderers[3] = transform.FindChild<SpriteRenderer>("Square_3");
    }
#endif
}
