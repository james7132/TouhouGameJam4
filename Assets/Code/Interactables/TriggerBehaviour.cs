using System;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class TriggerBehaviour : MonoBehaviour
{
    public event Action<TriggerBehaviour, Character> OnTriggerFired;

    protected void ClearListeners() 
    {
        OnTriggerFired = null;
    }

    protected void Invoke(Character character) 
    {
        OnTriggerFired?.Invoke(this, character);
    }

    // Called before the object is destroyed
    protected virtual void OnDestroy() 
    {
        ClearListeners();
    }
}
