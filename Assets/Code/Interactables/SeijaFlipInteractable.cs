using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeijaFlipInteractable : TriggerListener
{
    [SerializeField]
    private float _lerpSpeed = 40f;

    private float goalYScale;

    private void Start()
    {
        goalYScale = transform.localScale.y;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    OnTriggerFired(null, null);
        if (transform.localScale.y != goalYScale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale,
                new Vector3(transform.localScale.x, goalYScale, transform.localScale.z),
                Time.deltaTime * _lerpSpeed);
        }
    }

    protected override void OnTriggerFired(TriggerBehaviour trigger, Character source)
    {
        GetComponent<Rigidbody2D>().gravityScale *= -1f;
        goalYScale *= -1f;
    }
}
