#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;

/// <summary>
/// 오디오 미리듣기!
/// </summary>
public static class AudioEditorUtils
{
    public static void PlayClip(AudioClip clip)
    {
        if (clip == null) return;

        var method = typeof(AudioImporter).Assembly
            .GetType("UnityEditor.AudioUtil")
            .GetMethod("PlayPreviewClip", BindingFlags.Static | BindingFlags.Public, null,
                new[] { typeof(AudioClip), typeof(int), typeof(bool) }, null);

        method?.Invoke(null, new object[] { clip, 0, false });
    }

    public static void StopAllClips()
    {
        var method = typeof(AudioImporter).Assembly
            .GetType("UnityEditor.AudioUtil")
            .GetMethod("StopAllPreviewClips", BindingFlags.Static | BindingFlags.Public);

        method?.Invoke(null, null);
    }
}
#endif