using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// 유니티 에디터 전용 에셋 로더
/// </summary>
public static class AssetLoader
{
    /// <summary>
    /// GO 찾기
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject FindAndLoadByName(string name)
    {
        string[] guids = AssetDatabase.FindAssets($"t:Prefab {name}");

        if (guids.Length == 0)
        {
            Logger.Log($"{name} 프리팹 못 찾음");
            return null;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        return AssetDatabase.LoadAssetAtPath<GameObject>(path);
    }

    /// <summary>
    /// SO 찾기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T FindAndLoadByName<T>(string name) where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)} {name}");

        if (guids.Length == 0)
        {
            Logger.Log($"{name} SO 못 찾음");
            return null;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        return AssetDatabase.LoadAssetAtPath<T>(path);
    }
}
#endif