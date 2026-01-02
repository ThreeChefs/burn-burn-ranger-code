using UnityEngine;

public abstract class SceneSingletonManager<T> : MonoBehaviour where T : SceneSingletonManager<T>
{
    public static T Instance => instance;
    protected static T instance;


    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = (T)this;
    }


    public virtual void Init()
    {
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
