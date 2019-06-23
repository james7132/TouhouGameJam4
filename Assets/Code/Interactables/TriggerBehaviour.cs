using System;
using UnityEngine;

public abstract class TriggerBehaviour : MonoBehaviour, IInteractable
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

    public virtual void Interact(Character source) 
    {
        // TODO(james7132): Properly implement
        Invoke(source);
    }
}
