using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// 싱글톤 초기화 상태
    /// </summary>
    bool isInitialized = false;

    /// <summary>
    /// 싱글톤 종료 처리 상태
    /// </summary>
    private static bool isShutdown = false;

    /// <summary>
    /// 싱글톤 객체
    /// </summary>
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (isShutdown)
            {
                return null;
            }

            if (instance == null)
            {
                T singleton = FindAnyObjectByType<T>();             
                if (singleton == null)                              
                {
                    GameObject obj = new GameObject();              
                    obj.name = "Singleton";                         
                    singleton = obj.AddComponent<T>();              
                }
                instance = singleton;                               
                DontDestroyOnLoad(instance.gameObject);             
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)                                       
        {
            instance = this as T;                                   
            DontDestroyOnLoad(instance.gameObject);                 
        }
        else                                                        
        {
            if (instance != this)                                   
            {
                Destroy(this.gameObject);                           
            }
        }
    }

    private void OnEnable()
    {
                                                                    
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isInitialized)
        {
            OnPreInitialize();
        }

        if (mode != LoadSceneMode.Additive)
        {
            OnInitialize();
        }
    }

    protected virtual void OnPreInitialize()
    {
        isInitialized = true;
    }

    protected virtual void OnInitialize()
    {

    }

    private void OnApplicationQuit()
    {
        isShutdown = true;
    }
}
