using UnityEngine;

/// <summary>
/// 코루틴 러너
/// 씬 전환 시 파괴 허용
/// </summary>
public class CustomCoroutineRunner : MonoBehaviour
{
    private static CustomCoroutineRunner _instance;
    public static CustomCoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("CoroutineRunner");
                var coroutineRunner = obj.AddComponent<CustomCoroutineRunner>();
                _instance = coroutineRunner;
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }
}
