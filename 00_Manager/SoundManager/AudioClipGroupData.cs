using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



#if UNITY_EDITOR
using UnityEditorInternal;
#endif

[CreateAssetMenu(fileName = "AudioClipGroup", menuName = "SO/AudioClip Group")]
public class AudioClipGroupData : ScriptableObject
{
    [LabelText("사운드 | 볼륨 | 재생중첩제한")]
    public List<AudioClipEntry> tables;

    public int GetRandomClipIdx()
    {
        if (tables.Count == 0)
            return 0;

        int num = Define.Random.Next(0, tables.Count);
        return num;
    }

    public AudioClipEntry GetClip(int idx = 0)
    {
        if (tables.Count <= idx)
            return null;

        return tables[idx];
    }

    [PropertyOrder(-100)]
    [Button("■", ButtonSizes.Small)]
    private void Stop()
    {
#if UNITY_EDITOR
        AudioEditorUtils.StopAllClips();
#endif
    }
}

 [System.Serializable]
public class AudioClipEntry
{

    [HorizontalGroup("AudioClip", width: 0.3f)][HideLabel]
    [SerializeField] AudioClip _clip;
    public AudioClip Clip => _clip;

    [HorizontalGroup("AudioClip", width: 0.2f)][HideLabel]
    [SerializeField] float _volume = 1.0f;
    public float Volume => _volume;

    [HorizontalGroup("AudioClip", width: 0.2f)]
    [HideLabel]
    [SerializeField] float _limitInterval = 0.1f;
    public float LimitInterval => _limitInterval;


    [HorizontalGroup("AudioClip", width: 0.1f)]
    [Button("▶", ButtonSizes.Small)]
    private void Play()
    {
#if UNITY_EDITOR
        AudioEditorUtils.PlayClip(_clip);
#endif
    }
}