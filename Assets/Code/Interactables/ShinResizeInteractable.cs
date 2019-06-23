using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinResizeInteractable : TriggerListener
{
    [SerializeField]
    private int _sizeLevels = 3;
    [SerializeField]
    private int _currentSizeLevel = 0;
    [SerializeField]
    private float _sizeLevelUpMult = 2f;
    [SerializeField]
    private float _massLevelUpMult = 2f;

    protected override void OnTriggerFired(TriggerBehaviour trigger, Character source)
    {
        var scale = transform.localScale;
        var rb2d = GetComponent<Rigidbody2D>();
        _currentSizeLevel++;
        if (_currentSizeLevel < _sizeLevels)
        {
            scale *= _sizeLevelUpMult;
            rb2d.mass *= _massLevelUpMult;
        }
        else
        {
            for (int i = 0; i < _sizeLevels - 1; i++)
            {
                scale /= _sizeLevelUpMult;
                rb2d.mass /= _massLevelUpMult;
            }
            _currentSizeLevel = 0;
        }

        transform.localScale = scale;
    }
}
