using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FortuneBoxSlot : BaseSlot
{
    [SerializeField] GameObject _focus;

    [SerializeField] Image[] focusImgs;

    [SerializeField] Image _iconImg;

    SkillSelectDto _skill;
    public SkillSelectDto Skill => _skill;

    public void SetFocus(bool focus)
    {
        _focus.gameObject.SetActive(focus);

        if(focus)
        {
            foreach (Image f in focusImgs)
            {
                f.DOKill();
                Color start = f.color;
                start.a = 1;
                f.color = start;
            }
        }
    }

    public void SetFocusFade()
    {
        _focus.gameObject.SetActive(true);

        foreach (Image f in focusImgs)
        {
            Color start = f.color;
            start.a = 1;
            Color end = f.color;
            end.a = 0;

            f.color = start;
            f.DOColor(end, 0.25f).SetUpdate(true);
        }
    }


    public void SetSlot(SkillSelectDto dto)
    {
        _iconImg.sprite = dto.Icon;
    }



}
