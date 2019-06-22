using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Vector2 _interactionBoxSize;

    [SerializeField]
    Vector2 _interactionBoxOffset;

    [SerializeField]
    GameObject[] _enabledOnSelect;

    [SerializeField]
    float _movementAcc = 50;
    [SerializeField]
    float _movementAccAir = 30;
    [SerializeField]
    float _movementDec = 50;
    [SerializeField]
    float _movementDecAir = 0;
    [SerializeField]
    float _maxMoveSpeed = 5;
    [SerializeField]
    float _jumpForce = 5;

    [SerializeField]
    private Animator _rigAnimator;
    [SerializeField]
    private string _speedFloat = "Speed";
    [SerializeField]
    private string _runningBool = "Running";
    [SerializeField]
    private float _speedFloatWarmupAcc = 2f;
    [SerializeField]
    private float _speedFloatWarmupDec = 5f;
    [SerializeField]
    private string _turnAroundBool = "Turn";

    [SerializeField]
    private EdgeCollider2D _hitboxCollider;

#pragma warning restore 0649

    private Rigidbody2D _rb2d;
    private float animatorSpeedFloat = 0;
    HashSet<Collider2D> _characterColliders;

    public bool IsGrounded
    {
        get
        {
            var startPoint = (Vector2)transform.position + _hitboxCollider.points[0];
            var raycastVector = _hitboxCollider.points[1] - _hitboxCollider.points[0];
            Debug.DrawLine(startPoint, startPoint + raycastVector);
            var result = Physics2D.Raycast(startPoint, raycastVector.normalized, raycastVector.magnitude);
            return result && result.collider.tag.Equals("Floor");
        }
    }


    // Called before an object's first frame.
    void Start()
    {
        _characterColliders = new HashSet<Collider2D>(GetComponentsInChildren<Collider2D>());
        _rb2d = GetComponent<Rigidbody2D>();
        if (_hitboxCollider == null)
            _hitboxCollider = GetComponentInChildren<EdgeCollider2D>();
        if (_rigAnimator == null)
            _rigAnimator = GetComponentInChildren<Animator>();
        SetEnabledObjects(false);
    }

    /// <summary>
    /// Moves the character in a particular direction.
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector2 direction)
    {
        // Calculate acceleration
        var acc = 0f;
        if (direction.x == 0f)
            acc = IsGrounded ? _movementDec : _movementDecAir;
        else
            acc = IsGrounded ? _movementAcc : _movementAccAir;

        // Calculate goal X Speed
        var goalXSpeed = direction.x * _maxMoveSpeed;

        // Do velocity calculations
        var currentVelocity = _rb2d.velocity;
        var newXSpeed = Mathf.MoveTowards(currentVelocity.x, goalXSpeed, acc * Time.deltaTime);
        _rb2d.velocity = new Vector2(newXSpeed, currentVelocity.y);

        // Animation stuff
        var goalFloatValue = direction.x == 0f ? 0f : 1f;
        var animAcc = goalFloatValue == 0f ? _speedFloatWarmupDec : _speedFloatWarmupAcc;
        animatorSpeedFloat = Mathf.MoveTowards(animatorSpeedFloat, goalFloatValue, Time.deltaTime * animAcc);
        _rigAnimator.SetFloat(_speedFloat, animatorSpeedFloat);
        _rigAnimator.SetBool(_runningBool, animatorSpeedFloat > 0f);
        if (direction.x != 0f)
            _rigAnimator.SetBool(_turnAroundBool, direction.x < 0f);
    }

    public void Jump()
    {
        if (IsGrounded)
            _rb2d.AddForce(Vector2.up * _jumpForce);
    }

    /// <summary>
    /// Makes the character interact with all objects nearby.
    /// </summary>
    public void Interact()
    {
        Vector2 center = ((Vector2)transform.position) + _interactionBoxOffset;
        var interactables = Physics2D.OverlapBoxAll(center, _interactionBoxSize, 0)
                                     .Where(col => !_characterColliders.Contains(col))
                                     .SelectMany(col => col.GetComponentsInChildren<IInteractable>());
        foreach (var interactable in interactables)
        {
            try
            {
                interactable.Interact(this);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    public virtual void Select()
    {
        SetEnabledObjects(true);
    }

    public virtual void Deselect()
    {
        SetEnabledObjects(false);
    }

    void SetEnabledObjects(bool state)
    {
        foreach (var obj in _enabledOnSelect)
        {
            if (obj == null) continue;
            obj.SetActive(state);
        }
    }

}