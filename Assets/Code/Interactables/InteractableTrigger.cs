using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Triggers/Interactable Trigger")]
public class InteractableTrigger : TriggerBehaviour, IInteractable
{
    public void Interact(Character source) 
    {
        // TODO(james7132): Properly implement
        Invoke(source);
    }
}