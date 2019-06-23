using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeijaFlipInteractable : TriggerListener
{
    [SerializeField]
    private float _lerpSpeed = 10f;
    [SerializeField]
    private Transform _graphicTransform;

    private float goalYScale;

    private void Start()
    {
        goalYScale = _graphicTransform.localScale.y;
        if (_graphicTransform == null)
            _graphicTransform = GetComponentInChildren<SpriteRenderer>().transform;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //    OnTriggerFired(null, null);
        if (_graphicTransform.localScale.y != goalYScale)
        {
            _graphicTransform.localScale = Vector3.MoveTowards(_graphicTransform.localScale,
                new Vector3(_graphicTransform.localScale.x, goalYScale, _graphicTransform.localScale.z),
                Time.deltaTime * _lerpSpeed);
        }
    }

    protected override void OnTriggerFired(TriggerBehaviour trigger, Character source)
    {
        print(trigger);
        GetComponent<Rigidbody2D>().gravityScale *= -1f;
        goalYScale *= -1f;
        Debug.Log("Seija Flipped!");
    }
}
