using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerListener : MonoBehaviour
{
    #pragma warning disable 0649
    [SerializeField]
    List<TriggerBehaviour> _triggers;
    #pragma warning restore 0649

    public IReadOnlyCollection<TriggerBehaviour> Triggers => _triggers;

    void OnEnable() 
    {
        foreach (var trigger in _triggers) 
        {
            if (trigger == null) continue;
            trigger.OnTriggerFired += OnTriggerFiredImpl;
        }
    }

    void OnDisable() 
    {
        foreach (var trigger in _triggers) 
        {
            if (trigger == null) continue;
            trigger.OnTriggerFired -= OnTriggerFiredImpl;
        }
    }

    void OnTriggerFiredImpl(TriggerBehaviour trigger, Character character) 
    {
        if (IsValidTrigger(trigger, character)) 
        {
            OnTriggerFired(trigger, character);
        }
    }

    protected virtual bool IsValidTrigger(TriggerBehaviour trigger, Character character) => true;

    protected abstract void OnTriggerFired(TriggerBehaviour trigger, Character source);
}