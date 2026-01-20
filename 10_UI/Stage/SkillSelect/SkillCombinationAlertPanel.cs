using UnityEngine;
using UnityEngine.UI;

public class SkillCombinationAlertPanel : MonoBehaviour
{
    [SerializeField] RectTransform _layoutGroupRect;
    [SerializeField] Image[] _combiIcon;    // 더 많아질거면 생성해서 쓰기

    public void Init(SkillSelectDto skillDto)
    {
        foreach (var item in _combiIcon)
        {
            item.gameObject.SetActive(false);
        }

        for (int i = 0; i < skillDto.CombinationIcons.Length; ++i)
        {
            _combiIcon[i].gameObject.SetActive(true);
            _combiIcon[i].sprite = skillDto.CombinationIcons[i];
        }


        LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroupRect);
    }
}
