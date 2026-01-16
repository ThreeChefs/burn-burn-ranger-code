using TMPro;
using UnityEngine;

/// <summary>
/// UI - 스텟 바
/// </summary>
public class StatBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _value;
    [SerializeField] private StatType _statType;

    private void OnEnable()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.Condition[_statType].OnMaxValueChanged += UpdateStat;
        }
    }

    private void Start()
    {
        UpdateStat(PlayerManager.Instance.Condition[_statType].MaxValue);
    }

    private void OnDisable()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.Condition[_statType].OnMaxValueChanged -= UpdateStat;
        }
    }

    private void UpdateStat(float value)
    {
        _value.text = value.ToString();
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _value = transform.FindChild<TextMeshProUGUI>("Text (TMP) - Value");
    }
#endif
}
