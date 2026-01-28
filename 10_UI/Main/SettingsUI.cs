
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : PopupUI
{
    [Title("Settings UI")]
    [SerializeField] Slider _master;
    [SerializeField] Slider _sfx;
    [SerializeField] Slider _bgm;


    protected override void AwakeInternal()
    {
        base.AwakeInternal();

        _master.value = SoundManager.Instance.MasterVolume;
        _sfx.value = SoundManager.Instance.SfxVolume;
        _bgm.value = SoundManager.Instance.BgmVolume;


        _master.onValueChanged.AddListener(SoundManager.Instance.SetMasterVolume);
        _sfx.onValueChanged.AddListener(SoundManager.Instance.SetSfxVolume);
        _bgm.onValueChanged.AddListener(SoundManager.Instance.SetBgmVolume);

    }

}
