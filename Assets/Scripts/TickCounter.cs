using System;
using System.Collections;
using UnityEngine;

public class TickCounter : MonoBehaviour
{
    public delegate void TickEventHandler();
    public static TickCounter Instance { get; private set; }
    public event Action OnTick, EndTick;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError($"Multiple TickCounter instances detected. Destroying duplicate on {gameObject.name}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(TickEvent());
    }

    IEnumerator TickEvent()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.42f);
            OnTick?.Invoke();
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
