using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 아이템 슬롯 추상 클래스
/// </summary>
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
    // todo: 개수 넣기

    [Header("데이터")]
    [SerializeField] protected ItemData data;
    public ItemData Data => data;
    // todo: 레벨 등 동적으로 변하는 데이터 저장

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
    }

    protected virtual void OnClickButton()
    {
    }
    #endregion

    #region 슬롯 데이터 관리
    protected virtual void SetSlot(ItemData data)
    {
        this.data = data;

        SetItemClass();
        SetIcon();
    }

    private void SetItemClass()
    {
    }

    private void SetIcon()
    {
        icon.sprite = data.Icon;
    }
    #endregion

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        itemClass = transform.FindChild<Image>("Image - Class");
        icon = transform.FindChild<Image>("Image - Icon");
        level = transform.FindChild<TextMeshProUGUI>("Text (TMP) - Level");

        itemClass.color = ItemClassColor.GetGradeColor();
        icon.sprite = null;

        button = GetComponent<Button>();
        button.targetGraphic = itemClass;
    }
#endif
}
