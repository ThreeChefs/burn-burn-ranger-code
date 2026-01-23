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

    public void PlaySfx(SfxName sfxName, float pitch = 1.0f, int idx = 0, AudioSource aSource = null)
    {
        if (SFXTable.TryGetValue(sfxName, out AudioClipGroupData clip))
        {
            if (SfxLimiter.CanPlay(sfxName, idx, clip.GetClip().LimitInterval) == false) return;

            PlayInternal(SoundType.Sfx, clip, idx, aSource, false, pitch);
        }
    }

    public void PlaySfxRandom(SfxName sfxName, float pitch = 1.0f, AudioSource aSource = null)
    {
        if(SFXTable.TryGetValue(sfxName, out AudioClipGroupData clip))
        {
            int idx = clip.GetRandomClipIdx();

            if (SfxLimiter.CanPlay(sfxName, idx, clip.GetClip(idx).LimitInterval) == false) return;

            PlayInternal(SoundType.Sfx, clip, idx, aSource, false, pitch);

        }
    }

    public void PlayBgm(BgmName bgmName, int idx = 0, bool loop = true, float pitch = 1.0f, AudioSource aSource = null)
    {
        if (BgmTable.TryGetValue(bgmName, out AudioClipGroupData clip))
        {
            PlayInternal(SoundType.Bgm, clip, idx, aSource, loop, pitch);
        }
    }


    // Play 통합
    private void PlayInternal(SoundType type, AudioClipGroupData group, int idx, AudioSource audioSource, bool loop, float pitch)
    {
        if (group == null)
            return;

        AudioClip clip = group.GetClip(idx).Clip;
        float clipVolume = group.GetClip(idx).Volume;

        AudioSource target = audioSource != null ? 
            audioSource : (type == SoundType.Sfx ? GetSFXAudioSource() : _bgmAudioSource);

        float baseVolume = type == SoundType.Sfx ? _sfxVolume : _bgmVolume;
        baseVolume = baseVolume * _masterVolume;

        if (clip != null)
        {

            _sourceClipVolumes[target] = clipVolume;

            target.Stop();
            target.volume = baseVolume * clipVolume;
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
        _bgmAudioSource?.Stop();
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

    private Dictionary<AudioSource, float> _sourceClipVolumes = new(); // 재생중인 오디오 소스의 볼륨 

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
        RefreshVolumes();
    }

    public void SetSfxVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp(volume, 0, 1.0f);
        PlayerPrefs.SetFloat(_sfxVolumeName, MasterVolume);
        RefreshVolumes();
    }

    public void SetBgmVolume(float volume)
    {
        _bgmVolume = Mathf.Clamp(volume, 0, 1.0f);
        PlayerPrefs.SetFloat(_bgmVolumeName, MasterVolume);
        SetBgmVolume();
        RefreshVolumes();
    }

    private void RefreshVolumes()
    {
        if (_bgmAudioSource != null)
        {
            float clipVol = 1f;
            _sourceClipVolumes.TryGetValue(_bgmAudioSource, out clipVol);
            _bgmAudioSource.volume = _masterVolume * _bgmVolume * clipVol;
        }

        if (_sfxAudioSources != null)
        {
            foreach (AudioSource s in _sfxAudioSources)
            {
                if (s == null) continue;

                float clipVol = 1f;
                _sourceClipVolumes.TryGetValue(s, out clipVol);
                s.volume = _masterVolume * _sfxVolume * clipVol;
            }
        }
    }

    #endregion
}