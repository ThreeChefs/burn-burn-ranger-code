#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public static class BuffKeyGenerator
{
#if UNITY_EDITOR
    public static BuffKey Generate(ScriptableObject so)
    {
        var path = AssetDatabase.GetAssetPath(so);
        var guid = AssetDatabase.AssetPathToGUID(path);

        // GUID → int
        return new BuffKey(guid.GetHashCode());
    }
#endif
}
