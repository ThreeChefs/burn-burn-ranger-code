using UnityEngine;

/// <summary>
/// 상점 UI
/// </summary>
public class ShopUI : BaseUI
{
    [SerializeField] private PickUpSystem _pickupSystemPrefab;

    private void Awake()
    {
        Instantiate(_pickupSystemPrefab);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _pickupSystemPrefab = AssetLoader.FindAndLoadByName("PickUpSystem").GetComponent<PickUpSystem>();
    }
#endif
}
