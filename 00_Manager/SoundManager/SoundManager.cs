using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : GlobalSingletonManager<SoundManager>
{
    // 리소스에 넣지 않는 방식으로 변경
    [Header("사용할 클립 목록")] 
    [SerializeField] private AudioClipGroup[] sfxGroupAsset;
    [SerializeField] private AudioClipGroup[] bgmGroupAsset;

    [Header("Other Settings")]
    [SerializeField] int sfxAudioSourcePoolCount = 10;
    
    public float MasterVolume => masterVolume;
    public float SfxVolume => sfxVolume;
    public float BgmVolume => bgmVolume;
    private float sfxVolume = 1.0f;
    private float bgmVolume = 1.0f;
    private float masterVolume = 1.0f;

    static string sfxVolumeName = "SfxVolume";
    static string bgmVolumeName = "BgmVolume";
    static string masterVolumeName = "MasterVolume";


    Dictionary<SfxName, AudioClipGroup> SFXTable = new Dictionary<SfxName, AudioClipGroup>();
    Dictionary<BgmName, AudioClipGroup> BgmTable = new Dictionary<BgmName, AudioClipGroup>();

    AudioSource bgmAudioSource;
    private List<AudioSource> sfxAudioSources;


    // 리소스에서 옮기지 않는 방법으로 변경 필요
    protected override void Init()
    {
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        sfxAudioSources = new List<AudioSource>();


        // 일단 풀에 10개 넣어
        for (int i = 0; i < sfxAudioSourcePoolCount; ++i)
        {
            AudioSource aSource = gameObject.AddComponent<AudioSource>();
            aSource.playOnAwake = false;
            sfxAudioSources.Add(aSource);
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

        for (int i = 0; i < bgmGroupAsset.Length; i++)
        {
            if (Enum.TryParse(bgmGroupAsset[i].name, out BgmName bgmName))
            {
                BgmTable[bgmName] = bgmGroupAsset[i];
            }
        }

        for (int i = 0; i < sfxGroupAsset.Length; i++)
        {
            if (Enum.TryParse(sfxGroupAsset[i].name, ignoreCase: true, out SfxName sfxName))
            {
                SFXTable[sfxName] = sfxGroupAsset[i];
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

        PlaySound(GetSFXAudioSource(), aClip, GetVolume(sfxVolume, volume));
    }

    public void PlaySfxRandom(SfxName sfxName, float volume = 1.0f)
    {
        AudioClip aClip = null;
        if (SFXTable.TryGetValue(sfxName, out AudioClipGroup clip))
        {
            aClip = clip.GetRandomClip();
        }

        PlaySound(GetSFXAudioSource(), aClip, GetVolume(sfxVolume, volume));
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

        PlaySound(aSource, aClip, GetVolume(sfxVolume, volume));
    }

    public void PlayBgmAtAudioSource(AudioSource aSource, BgmName bgmName, int idx = 0, float volume = 1.0f,
        bool loop = true, float pitch = 1.0f)
    {
        AudioClip aClip = null;
        if (BgmTable.TryGetValue(bgmName, out AudioClipGroup clip))
        {
            aClip = clip.GetClip(idx);
        }

        PlaySound(aSource, aClip, GetVolume(bgmVolume, volume), loop, pitch);
    }

    public void PlayBgm(BgmName bgmName, float volume = 1.0f, bool loop = true, float pitch = 1.0f)
    {
        AudioClip aClip = null;
        if (BgmTable.TryGetValue(bgmName, out AudioClipGroup clip))
        {
            aClip = clip.GetClip();
        }

        PlaySound(bgmAudioSource, aClip, GetVolume(bgmVolume, volume), loop, pitch);
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
        bgmAudioSource.pitch = pitch;
    }

    public void AllStop()
    {
        StopBgm();
        StopAllSfx();
    }

    public void StopBgm()
    {
        bgmAudioSource.Stop();
    }

    public void StopAllSfx()
    {
        foreach (AudioSource aSource in sfxAudioSources)
        {
            aSource.Stop();
        }
    }
    
    

    AudioSource GetSFXAudioSource()
    {
        AudioSource shortestSource = null;
        float shortestRemainTime = float.MaxValue;

        foreach (AudioSource audioSource in sfxAudioSources)
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
        bgmAudioSource.volume = GetVolume(BgmVolume, 1.0f);
    }

    void SetSavedVolume()
    {
        masterVolume = PlayerPrefs.GetFloat(masterVolumeName, 1.0f);
        sfxVolume = PlayerPrefs.GetFloat(sfxVolumeName, 1.0f);
        bgmVolume = PlayerPrefs.GetFloat(bgmVolumeName, 1.0f);
    }

    float GetVolume(float settingVolume, float customVolume)
    {
        return MasterVolume * settingVolume * customVolume;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp(volume, 0, 1.0f);
        PlayerPrefs.SetFloat(masterVolumeName, MasterVolume);
        SetBgmVolume();
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = Mathf.Clamp(volume, 0, 1.0f);
        PlayerPrefs.SetFloat(sfxVolumeName, MasterVolume);
    }

    public void SetBgmVolume(float volume)
    {
        bgmVolume = Mathf.Clamp(volume, 0, 1.0f);
        PlayerPrefs.SetFloat(bgmVolumeName, MasterVolume);
        SetBgmVolume();
    }

    #endregion
}