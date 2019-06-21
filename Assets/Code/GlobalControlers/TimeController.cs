using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }

    public bool IsPaused { get; private set; }

    [SerializeField]
    KeyCode _pauseKey = KeyCode.Escape;

    [SerializeField]
    float _defaultTimeScale = 1.0f;

    void Awake() 
    {
        Instance = this;
        IsPaused = false;
        Time.timeScale = _defaultTimeScale;
    }

    void Update()
    { 
        if (!Input.GetKeyDown(_pauseKey)) return;
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0.0f : _defaultTimeScale;
        Debug.Log($"Paused State: {IsPaused}");
    }
}
