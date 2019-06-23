using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingMusic : MonoBehaviour
{
    
    [SerializeField]
    private float loopStartTime;
    [SerializeField]
    private float loopEndTime;
    [SerializeField]
    private bool startAtLoopPoint;
    [SerializeField]
    private bool testLoopPoint;

    private AudioSource _audioSource;

	void Awake()
	{
        _audioSource = GetComponent<AudioSource>();
        if (testLoopPoint)
            _audioSource.time = loopEndTime - 5f;
        else if (startAtLoopPoint)
            _audioSource.time =  loopStartTime;
    }
	
	void Update()
	{
        if (_audioSource.time >= loopEndTime)
            _audioSource.time -= loopEndTime - loopStartTime;
	}
}
