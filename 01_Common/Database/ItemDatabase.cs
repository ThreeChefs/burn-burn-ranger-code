using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "SO/Database/Item Database")]
public class ItemDatabase : SoDatabase
{
    public ItemData FindById(int id)
    {
        foreach (ItemData item in List)
        {
            if (item.Id == id) return item;
        }

        return null;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        FindItems();
        SortId();
    }

    [Button("아이템 가져오기")]
    private void FindItems()
    {
        AssetLoader.FindAndLoadAllByType<ItemData>().ForEach(data =>
        {
            if (!List.Contains(data))
            {
                List.Add(data);
            }
        });
    }

    [Button("id로 정렬")]
    private void SortId()
    {
        List.Sort((a, b) => ((ItemData)a).Id.CompareTo(((ItemData)b).Id));
    }
#endif
}
