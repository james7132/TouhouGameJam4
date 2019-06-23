using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeijaFlipInteractable : TriggerListener
{
    protected override void OnTriggerFired(TriggerBehaviour trigger, Character source)
    {
        GetComponent<Rigidbody2D>().gravityScale *= -1f;
    }
}
