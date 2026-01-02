using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GlobalSingletonManager<T> : MonoBehaviour where T : GlobalSingletonManager<T>
{
    public static T Instance => instance;
    private static T instance;
   

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = (T)this;
        DontDestroyOnLoad(this.gameObject);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded +=  OnSceneUnloaded;
        
        Init();
    }


    protected virtual void Init()
    {
    }

    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      
    }

    protected virtual void OnSceneUnloaded(Scene scene)
    {
        
    }

}
