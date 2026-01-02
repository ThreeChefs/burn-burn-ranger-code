using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SceneDatabase", menuName = "SO/Scene Database")]
public class SceneDatabase : ScriptableObject
{
    public List<SceneEntry> scenes;

    Dictionary<SceneType, GameObject> _cache;

    private void OnEnable()
    {
        _cache = new();
        foreach (var scene in scenes)
        {
            if (!_cache.ContainsKey(scene.type))
            {
                _cache.Add(scene.type, scene.prefab);
            }
        }
    }

    public bool TryGetScene(SceneType type, out GameObject prefab)
    {
        return _cache.TryGetValue(type, out prefab);
    }
}

[System.Serializable]
public class SceneEntry
{
    public SceneType type;
    public GameObject prefab;
}
