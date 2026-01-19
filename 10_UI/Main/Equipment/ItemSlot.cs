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

    protected ItemInstance instance;

    #region Unity API
    protected void Awake()
    {
        Init();
    }

    protected virtual void OnEnable()
    {
        button.onClick.AddListener(OnClickButton);
    }

    protected virtual void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }

    private void OnDestroy()
    {
        ResetSlot();
    }
    #endregion

    #region 초기화
    protected virtual void Init()
    {
        if (instance == null)
        {
            itemClass.gameObject.SetActive(false);
            icon.gameObject.SetActive(false);
            level.gameObject.SetActive(false);
            count.gameObject.SetActive(false);
        }
    }

    protected virtual void OnClickButton()
    {
        SoundManager.Instance.PlaySfx(SfxName.Sfx_Click);
    }
    #endregion

    #region 슬롯 데이터 관리
    /// <summary>
    /// [public] 슬롯에 아이템 정보 넣기
    /// </summary>
    /// <param name="itemInstance"></param>
    public virtual void SetSlot(ItemInstance itemInstance)
    {
        if (instance != null)
        {
            if (instance.Equals(itemInstance)) return;
            instance.OnLevelChanged -= UpdateLevel;
        }
        instance = itemInstance;
        instance.OnLevelChanged += UpdateLevel;

        SetActiveComponent(true);

        itemClass.color = ItemUtils.GetClassColor(instance.ItemClass);
        icon.sprite = instance.ItemData.Icon;
        count.text = instance.Count < 2 ? "" : instance.Count.ToString();
        UpdateLevel();
    }

    public virtual void ResetSlot()
    {
        if (instance != null)
        {
            instance.OnLevelChanged -= UpdateLevel;
        }
        instance = null;

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
        return instance == null;
    }

    private void UpdateLevel()
    {
        level.text = instance.Level.ToString();
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
