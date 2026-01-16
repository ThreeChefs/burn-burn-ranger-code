using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 아이템 슬롯 추상 클래스
/// </summary>
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Button))]
[System.Serializable]
public class ItemSlot : MonoBehaviour
{
    [Header("컴포넌트")]
    [SerializeField] protected Button button;

    [Header("자식 컴포넌트")]
    [SerializeField] protected Image itemClass;
    [SerializeField] protected Image icon;
    [SerializeField] protected TextMeshProUGUI level;
    [SerializeField] protected TextMeshProUGUI count;

    private ItemInstance _itemInstance;

    #region Unity API
    protected void Awake()
    {
        Init();
    }

    protected void OnEnable()
    {
        button.onClick.AddListener(OnClickButton);
    }

    protected void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
    #endregion

    #region 초기화
    protected virtual void Init()
    {
        if (_itemInstance == null)
        {
            itemClass.gameObject.SetActive(false);
            icon.gameObject.SetActive(false);
            level.gameObject.SetActive(false);
            count.gameObject.SetActive(false);
        }
    }

    protected virtual void OnClickButton()
    {
        ItemDetailUI ui = UIManager.Instance.ShowUI(UIName.UI_ItemDetail) as ItemDetailUI;
        ui.SetItem(_itemInstance);
    }
    #endregion

    #region 슬롯 데이터 관리
    /// <summary>
    /// [public] 슬롯에 아이템 정보 넣기
    /// </summary>
    /// <param name="itemInstance"></param>
    public virtual void SetSlot(ItemInstance itemInstance)
    {
        if (_itemInstance != null && _itemInstance.Equals(itemInstance)) return;

        _itemInstance = itemInstance;

        SetActiveComponent(true);

        itemClass.color = ItemUtils.GetClassColor(itemInstance.ItemClass);
        icon.sprite = itemInstance.ItemData.Icon;
        level.text = itemInstance.Level.ToString();
        count.text = itemInstance.Count < 2 ? "" : itemInstance.Count.ToString();
    }

    public virtual void ResetSlot()
    {
        _itemInstance = null;

        SetActiveComponent(false);
    }

    private void SetActiveComponent(bool active)
    {
        itemClass.gameObject.SetActive(active);
        icon.gameObject.SetActive(active);
        level.gameObject.SetActive(active);
        count.gameObject.SetActive(active);
    }

    public bool IsEmpty()
    {
        return _itemInstance == null;
    }
    #endregion

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        transform.FindOrInstantiate(ref itemClass, "Image - Class");
        transform.FindOrInstantiate(ref icon, "Image - Icon");
        transform.FindOrInstantiate(ref level, "Text (TMP) - Level");
        transform.FindOrInstantiate(ref count, "Text (TMP) - Count");

        itemClass.color = ItemUtils.GetClassColor();
        icon.sprite = null;

        button = GetComponent<Button>();
        button.targetGraphic = itemClass;
    }
#endif
}
