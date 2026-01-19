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


    Dictionary<SfxName, AudioClipGroupData> SFXTable = new Dictionary<SfxName, AudioClipGroupData>();
    Dictionary<BgmName, AudioClipGroupData> BgmTable = new Dictionary<BgmName, AudioClipGroupData>();

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
            part => (AudioClipGroupData)null);

        BgmTable = ((BgmName[])Enum.GetValues(typeof(BgmName))).ToDictionary(part => part,
            part => (AudioClipGroupData)null);


        List<AudioClipGroupData> bgmGroupAssetList = _bgmGroupAssetTable.GetDatabase<AudioClipGroupData>();
        List<AudioClipGroupData> sfxGroupAssetList = _sfxGroupAssetTable.GetDatabase<AudioClipGroupData>();

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

    public void PlaySfx(SfxName sfxName, float volume = 1.0f, float pitch = 1.0f, int idx = 0, AudioSource aSource = null)
    {
        if (SFXTable.TryGetValue(sfxName, out AudioClipGroupData clip))
        {
            PlayInternal(SoundType.Sfx, clip, idx, aSource, volume, false, pitch);
        }
    }

    public void PlaySfxRandom(SfxName sfxName, float volume = 1.0f, float pitch = 1.0f, AudioSource aSource = null)
    {
        if (SFXTable.TryGetValue(sfxName, out AudioClipGroupData clip))
        {
            PlayInternal(SoundType.Sfx, clip, -1, aSource, volume, false, pitch);
        }

    }

    public void PlayBgm(BgmName bgmName, int idx = 0, float volume = 1.0f, bool loop = true, float pitch = 1.0f, AudioSource aSource = null)
    {
        if (BgmTable.TryGetValue(bgmName, out AudioClipGroupData clip))
        {
            PlayInternal(SoundType.Bgm, clip, idx, aSource, volume, false, pitch);
        }
    }


    // Play 통합
    private void PlayInternal(SoundType type, AudioClipGroupData group, int clipIndex, AudioSource audioSource, float volume, bool loop, float pitch)
    {
        if (group == null)
            return;

        AudioClip clip = clipIndex < 0 ? group.GetRandomClip() : group.GetClip(clipIndex);
        AudioSource target = audioSource != null ? 
            audioSource : (type == SoundType.Sfx ? GetSFXAudioSource() : _bgmAudioSource);

        float baseVolume = type == SoundType.Sfx ? _sfxVolume : _bgmVolume;

        if (clip != null)
        {
            target.Stop();
            target.volume = volume;
            target.clip = clip;
            target.loop = loop;
            target.pitch = pitch;
            target.Play();
        }
    }



    public void SetNowBgmPitch(float pitch)
    {
        _bgmAudioSource.pitch = pitch;
    }


    #region 정지

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

    #endregion


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