using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinResizeInteractable : TriggerListener
{
    [SerializeField]
    private int _sizeLevels = 3;
    [SerializeField]
    private int _currentSizeLevel = 1;
    [SerializeField]
    private float _sizeLevelUpMult = 2f;
    [SerializeField]
    private float _massLevelUpMult = 2f;
    [SerializeField]
    private float _lerpSizeSpeed = 20f;

    private Vector3 goalScale;

    private void Start()
    {
        goalScale = transform.localScale;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //    OnTriggerFired(null, null);
        if (transform.localScale != goalScale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, goalScale, Time.deltaTime * _lerpSizeSpeed);
        }
    }

    protected override void OnTriggerFired(TriggerBehaviour trigger, Character source)
    {
        print((CharacterSpecificTrigger)trigger);
        var rb2d = GetComponent<Rigidbody2D>();
        _currentSizeLevel++;
        if (_currentSizeLevel < _sizeLevels)
        {
            goalScale *= _sizeLevelUpMult;
            rb2d.mass *= _massLevelUpMult;
        }
        else
        {
            for (int i = 0; i < _sizeLevels - 1; i++)
            {
                goalScale /= _sizeLevelUpMult;
                rb2d.mass /= _massLevelUpMult;
            }
            _currentSizeLevel = 0;
        }
        Debug.Log("Shin resized!");
    }
}
