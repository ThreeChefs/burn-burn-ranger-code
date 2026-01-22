using System.Collections.Generic;
using UnityEngine;

public static class SfxLimiter
{
    private static readonly Dictionary<int, float> _lastPlayTime = new();

    public static bool CanPlay(SfxName name, int sfxId, float interval)
    {
        int key = MakeKey(name, sfxId);
        float now = Time.unscaledTime;

        if (_lastPlayTime.TryGetValue(key, out float last) &&
            now - last < interval)
            return false;

        _lastPlayTime[key] = now;
        return true;
    }

    private static int MakeKey(SfxName name, int sfxId)
    {
        return ((int)name << 16) | (sfxId & 0xFFFF);
    }
}
