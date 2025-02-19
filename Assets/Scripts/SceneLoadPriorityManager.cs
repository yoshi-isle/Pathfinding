using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneLoadPriority
{
    int SceneLoadPriority { get; }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode);
}

public class SceneLoadPriorityManager : MonoBehaviour
{
    private static SceneLoadPriorityManager _instance;
    public static SceneLoadPriorityManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singleton = new GameObject(typeof(SceneLoadPriorityManager).Name);
                _instance = singleton.AddComponent<SceneLoadPriorityManager>();
                DontDestroyOnLoad(singleton);
            }

            return _instance;
        }
    }

    private List<ISceneLoadPriority> _listeners = new List<ISceneLoadPriority>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void RegisterListener(ISceneLoadPriority listener)
    {
        _listeners.Add(listener);
        _listeners.Sort((a, b) => a.SceneLoadPriority.CompareTo(b.SceneLoadPriority));
    }

    public void DeregisterListener(ISceneLoadPriority listener)
    {
        _listeners.Remove(listener);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var listener in _listeners)
        {
            listener.OnSceneLoaded(scene, mode);
        }
    }
}
