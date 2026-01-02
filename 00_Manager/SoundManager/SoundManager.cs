using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : GlobalSingletonManager<SoundManager>
{
    // SODatabase 로 프리팹 수정하지 않고 사용할 수 있도록 변경
    [Header("사용할 클립 목록")]
    [SerializeField] private SoDatabase _sfxGroupAssetTable;
    [SerializeField] private SoDatabase _bgmGroupAssetTable;
    
    [Header("Other Settings")]
    [SerializeField] int _sfxAudioSourcePoolCount = 10;
    
    public float MasterVolume => _masterVolume;
    public float SfxVolume => _sfxVolume;
    public float BgmVolume => _bgmVolume;
    private float _sfxVolume = 1.0f;
    private float _bgmVolume = 1.0f;
    private float _masterVolume = 1.0f;

    static string _sfxVolumeName = "SfxVolume";
    static string _bgmVolumeName = "BgmVolume";
    static string _masterVolumeName = "MasterVolume";


    Dictionary<SfxName, AudioClipGroup> SFXTable = new Dictionary<SfxName, AudioClipGroup>();
    Dictionary<BgmName, AudioClipGroup> BgmTable = new Dictionary<BgmName, AudioClipGroup>();

    AudioSource _bgmAudioSource;
    private List<AudioSource> _sfxAudioSources;
    
    
    protected override void Init()
    {
        _bgmAudioSource = gameObject.AddComponent<AudioSource>();
        _sfxAudioSources = new List<AudioSource>();


        // 일단 풀에 10개 넣어
        for (int i = 0; i < _sfxAudioSourcePoolCount; ++i)
        {
            AudioSource aSource = gameObject.AddComponent<AudioSource>();
            aSource.playOnAwake = false;
            _sfxAudioSources.Add(aSource);
        }

        ReadAudioClips();
        SetSavedVolume();
    }

    void ReadAudioClips()
    {
        SFXTable = ((SfxName[])Enum.GetValues(typeof(SfxName))).ToDictionary(part => part,
            part => (AudioClipGroup)null);

        BgmTable = ((BgmName[])Enum.GetValues(typeof(BgmName))).ToDictionary(part => part,
            part => (AudioClipGroup)null);
        
        
        List<AudioClipGroup> bgmGroupAssetList = _bgmGroupAssetTable.GetDatabase<AudioClipGroup>();
        List<AudioClipGroup> sfxGroupAssetList = _sfxGroupAssetTable.GetDatabase<AudioClipGroup>();
        
        for (int i = 0; i < bgmGroupAssetList.Count; i++)
        {
            if (Enum.TryParse(bgmGroupAssetList[i].name, out BgmName bgmName))
            {
                BgmTable[bgmName] = bgmGroupAssetList[i];
            }
        }

        for (int i = 0; i < sfxGroupAssetList.Count; i++)
        {
            if (Enum.TryParse(sfxGroupAssetList[i].name, ignoreCase: true, out SfxName sfxName))
            {
                SFXTable[sfxName] = sfxGroupAssetList[i];
            }
        }
    }
    
    public void PlaySfxOnce(SfxName sfxName, float volume = 1.0f, int idx = 0)
    {
        AudioClip aClip = null;
        if (SFXTable.TryGetValue(sfxName, out AudioClipGroup clip))
        {
            aClip = clip.GetClip(idx);
        }

        PlaySound(GetSFXAudioSource(), aClip, GetVolume(_sfxVolume, volume));
    }

    public void PlaySfxRandom(SfxName sfxName, float volume = 1.0f)
    {
        AudioClip aClip = null;
        if (SFXTable.TryGetValue(sfxName, out AudioClipGroup clip))
        {
            aClip = clip.GetRandomClip();
        }

        PlaySound(GetSFXAudioSource(), aClip, GetVolume(_sfxVolume, volume));
    }

    /// <summary>
    /// 3D 환경의 특정 AudioSource 에서 재생할 때 사용
    /// </summary>
    public void PlaySfxOnceAtAudioSource(AudioSource aSource, SfxName sfxName, int idx = 0, float volume = 1.0f)
    {
        AudioClip aClip = null;
        if (SFXTable.TryGetValue(sfxName, out AudioClipGroup clip))
        {
            aClip = clip.GetClip(idx);
        }

        PlaySound(aSource, aClip, GetVolume(_sfxVolume, volume));
    }

    public void PlayBgmAtAudioSource(AudioSource aSource, BgmName bgmName, int idx = 0, float volume = 1.0f,
        bool loop = true, float pitch = 1.0f)
    {
        AudioClip aClip = null;
        if (BgmTable.TryGetValue(bgmName, out AudioClipGroup clip))
        {
            aClip = clip.GetClip(idx);
        }

        PlaySound(aSource, aClip, GetVolume(_bgmVolume, volume), loop, pitch);
    }

    public void PlayBgm(BgmName bgmName, float volume = 1.0f, bool loop = true, float pitch = 1.0f)
    {
        AudioClip aClip = null;
        if (BgmTable.TryGetValue(bgmName, out AudioClipGroup clip))
        {
            aClip = clip.GetClip();
        }

        PlaySound(_bgmAudioSource, aClip, GetVolume(_bgmVolume, volume), loop, pitch);
    }


    void PlaySound(AudioSource audioSource, AudioClip aClip, float volume = 1.0f, bool loop = false, float pitch = 1.0f)
    {
        if (audioSource == null) return;

        if (aClip != null)
        {
            audioSource.Stop();
            audioSource.volume = volume;
            audioSource.clip = aClip;
            audioSource.loop = loop;
            audioSource.pitch = pitch;
            audioSource.Play();
        }
    }
    
    
    
    public void SetNowBgmPitch(float pitch)
    {
        _bgmAudioSource.pitch = pitch;
    }

    public void AllStop()
    {
        StopBgm();
        StopAllSfx();
    }

    public void StopBgm()
    {
        _bgmAudioSource.Stop();
    }

    public void StopAllSfx()
    {
        foreach (AudioSource aSource in _sfxAudioSources)
        {
            aSource.Stop();
        }
    }
    
    

    AudioSource GetSFXAudioSource()
    {
        AudioSource shortestSource = null;
        float shortestRemainTime = float.MaxValue;

        foreach (AudioSource audioSource in _sfxAudioSources)
        {
            if (audioSource.isPlaying == false)
            {
                return audioSource;
            }

            // 제일 짧게 남은거 끊고 재생하는 방식
            float remainTime = audioSource.clip.length - audioSource.time;
            if (remainTime <= shortestRemainTime)
            {
                shortestRemainTime = remainTime;
                shortestSource = audioSource;
            }
        }

        return shortestSource;

        // 스킵하지 않고 audioSource 추가하는 버전
        // AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        // sfxAudioSources.Add(newAudioSource);
        // return newAudioSource;
    }


    #region VolumeSettings
    
    void SetBgmVolume()
    {
        _bgmAudioSource.volume = GetVolume(BgmVolume, 1.0f);
    }

    void SetSavedVolume()
    {
        _masterVolume = PlayerPrefs.GetFloat(_masterVolumeName, 1.0f);
        _sfxVolume = PlayerPrefs.GetFloat(_sfxVolumeName, 1.0f);
        _bgmVolume = PlayerPrefs.GetFloat(_bgmVolumeName, 1.0f);
    }

    float GetVolume(float settingVolume, float customVolume)
    {
        return MasterVolume * settingVolume * customVolume;
    }

    public void SetMasterVolume(float volume)
    {
        _masterVolume = Mathf.Clamp(volume, 0, 1.0f);
        PlayerPrefs.SetFloat(_masterVolumeName, MasterVolume);
        SetBgmVolume();
    }

    public void SetSfxVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp(volume, 0, 1.0f);
        PlayerPrefs.SetFloat(_sfxVolumeName, MasterVolume);
    }

    public void SetBgmVolume(float volume)
    {
        _bgmVolume = Mathf.Clamp(volume, 0, 1.0f);
        PlayerPrefs.SetFloat(_bgmVolumeName, MasterVolume);
        SetBgmVolume();
    }

    #endregion
}