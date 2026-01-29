using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FortuneBoxSlot : BaseSlot
{
    [SerializeField] GameObject _focus;
    [SerializeField] Image[] focusImgs;
    [SerializeField] Image _iconImg;

    [SerializeField] Sprite _foodSpr;
    [SerializeField] Sprite _goldSpr;
    

    SkillSelectDto _skill;
    public SkillSelectDto Skill => _skill;

    FortuneBoxSlotType _slotType;
    public FortuneBoxSlotType SlotType => _slotType;
    

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
            f.DOKill();
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
        _skill = dto;
        _iconImg.sprite = dto.Icon;
        _slotType = FortuneBoxSlotType.Skill;
    }

    public void SetSlot(FortuneBoxSlotType type)
    {
        _slotType = type;

        switch (type)
        { 
            case FortuneBoxSlotType.Food:
                iconImg.sprite = _foodSpr;
                break;

            case FortuneBoxSlotType.Gold:
                iconImg.sprite = _goldSpr;
                break;
        }
    }



}

public enum FortuneBoxSlotType
{
    Skill,
    Food,
    Gold,
}