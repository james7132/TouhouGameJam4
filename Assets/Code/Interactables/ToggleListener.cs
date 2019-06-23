using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleListener : TriggerListener
{
    [SerializeField]
    GameObject[] _toggleObjects;

    protected override void OnTriggerFired(TriggerBehaviour trigger, Character source)
    {
        foreach (var obj in _toggleObjects) 
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
