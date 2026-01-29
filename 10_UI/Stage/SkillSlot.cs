using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] bool _useIcon;
    [ShowIf("_useIcon")][SerializeField] Image _icon;

    [SerializeField] bool _useSkillLevelPanel;
    [ShowIf("_useSkillLevelPanel")][SerializeField] SkillLevelPanel _skillLevelPanel;

    [SerializeField] bool _useSkillName;
    [ShowIf("_useSkillName")][SerializeField] TextMesh _skillName;

    [SerializeField] bool _useSKillDamageRate;
    [ShowIf("_useSKillDamageRate")][SerializeField] Slider _damageRateSlider;
    [ShowIf("_useSKillDamageRate")][SerializeField] TextMeshProUGUI _damageRateText;
    [ShowIf("_useSKillDamageRate")][SerializeField] BigIntText _damageText;


    public void SetSkillElement(BaseSkill skill)
    {

        if (_useIcon && _icon != null)
        {
            _icon.gameObject.SetActive(true);
            _icon.sprite = skill.SkillData.Icon;
        }

        if (_useSkillLevelPanel && _skillLevelPanel != null)
        {
            _skillLevelPanel.gameObject.SetActive(true);
            _skillLevelPanel.Init(skill.SkillData.Type, skill.CurLevel, false);
        }

        if(_useSkillName && _skillName != null)
        {
            _skillName.text = skill.SkillData.DisplayName;
        }

        if(_useSKillDamageRate)
        {
            SetSkillDamageRate(skill);
        }

    }

    public void SetEmpty()
    {
        if (_icon != null)
        {
            _icon.gameObject.SetActive(false);
        }

        if (_skillLevelPanel != null)
        {
            _skillLevelPanel.gameObject.SetActive(false);
        }

        if (_skillName != null)
        {
            _skillName.gameObject.SetActive(false);
        }


        if (_damageRateSlider != null) _damageRateSlider.gameObject.SetActive(false);
        if (_damageText != null) _damageText.gameObject.SetActive(false);
        if(_damageRateText != null) _damageRateText.gameObject.SetActive(false);
    }


    public void SetSkillDamageRate(BaseSkill skill)
    {

        DamageStatus ds = PlayerManager.Instance.StagePlayer.DamageStatus;

        float totalDmg = 0f;
        foreach (float damage in ds._totalDamage.Values)
        {
            totalDmg += damage;
        }


        if (ds._totalDamage.ContainsKey(skill.SkillData.RuntimeIndex))
        {
         
            float skillDmg = ds._totalDamage[skill.SkillData.RuntimeIndex];

            
            if (_damageRateSlider != null)
            {
                _damageRateSlider.gameObject.SetActive(true);
                _damageRateSlider.value = skillDmg/totalDmg;
            }

            if (_damageText != null)
            {
                _damageText.gameObject.SetActive(true);
                _damageText.SetValue((int)skillDmg);
            }

            if (_damageRateText != null)
            {
                _damageRateText.gameObject.SetActive(true);
                _damageRateText.text = $"{((skillDmg / totalDmg)*100):F0}%";
            }

        }
        else
        {
            if (_damageRateSlider != null)
            {
                _damageRateSlider.gameObject.SetActive(true);
                _damageRateSlider.value = 0;
            }

            if (_damageText != null)
            {
                _damageText.gameObject.SetActive(true);
                _damageText.SetValue(0);
            }

            if (_damageRateText != null)
            {
                _damageRateText.gameObject.SetActive(true);
                _damageRateText.text = $"0%";
            }
        }



      

    }
}
