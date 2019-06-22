using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
#pragma warning disable 0649
    [Header("Controls")]
    [SerializeField]
    string _horizontalAxis = "Horizontal";
    [SerializeField]
    string _verticalAxis = "Vertical";

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
    LayerMask groundDetectionMask;

    [SerializeField]
    private Animator _rigAnimator;
    [SerializeField]
    private string _speedFloat = "Speed";
    [SerializeField]
    private string _runningBool = "Running";
    [SerializeField]
    private string _JumpStateInt = "JumpState"; // 0 is grounded, 1 is jumping up, 2 is falling down
    [SerializeField]
    private string _turnAroundBool = "Turn";
    [SerializeField]
    private float _speedFloatWarmupAcc = 2f;
    [SerializeField]
    private float _speedFloatWarmupDec = 5f;

    [SerializeField]
    private EdgeCollider2D _hitboxCollider;

#pragma warning restore 0649

    private Rigidbody2D _rb2d;
    private float animatorSpeedFloat = 0;
    HashSet<Collider2D> _characterColliders;
    public bool IsSelected { get; set; }

    public bool IsGrounded
    {
        get
        {
            var startPoint = (Vector2)transform.position + _hitboxCollider.points[0];
            var raycastVector = _hitboxCollider.points[1] - _hitboxCollider.points[0];
            Debug.DrawLine(startPoint, startPoint + raycastVector);
            var result = Physics2D.Raycast(startPoint, raycastVector.normalized, raycastVector.magnitude, groundDetectionMask);
            return result;
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

    private void Update()
    {
        var movement = IsSelected
                            ? new Vector2(Input.GetAxisRaw(_horizontalAxis),
                                   Input.GetAxisRaw(_verticalAxis))
                            : Vector2.zero;
        HandleMovement(movement);
        if (Input.GetButtonDown(_verticalAxis) && movement.y > 0)
        {
            Jump();
        }
        HandleAnimation(movement);
    }

    /// <summary>
    /// Moves the character in a particular direction.
    /// </summary>
    /// <param name="direction"></param>
    public void HandleMovement(Vector2 direction)
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
    }

    void HandleAnimation(Vector2 direction)
    {
        // Animation stuff
        // Run
        var goalFloatValue = direction.x == 0f ? 0f : 1f;
        var animAcc = goalFloatValue == 0f ? _speedFloatWarmupDec : _speedFloatWarmupAcc;
        animatorSpeedFloat = Mathf.MoveTowards(animatorSpeedFloat, goalFloatValue, Time.deltaTime * animAcc);
        _rigAnimator.SetFloat(_speedFloat, animatorSpeedFloat);
        _rigAnimator.SetBool(_runningBool, animatorSpeedFloat > 0f);
        // Flip
        if (direction.x != 0f)
            _rigAnimator.SetBool(_turnAroundBool, direction.x < 0f);
        // Jump
        if (IsGrounded)
            _rigAnimator.SetInteger(_JumpStateInt, 0);
        else if (_rb2d.velocity.y > 0f)
            _rigAnimator.SetInteger(_JumpStateInt, 1);
        else
            _rigAnimator.SetInteger(_JumpStateInt, 2);
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
        IsSelected = true;
        SetEnabledObjects(true);
    }

    public virtual void Deselect()
    {
        IsSelected = false;
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