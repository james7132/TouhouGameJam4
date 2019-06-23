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

    [Header("Movement Physics")]
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
    LayerMask _groundDetectionMask;
    [SerializeField]
    LayerMask _sideDetectionMask;

    [Header("Animation")]
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
    private string _interactTrigger = "Interact";
    [SerializeField]
    private float _speedFloatWarmupAcc = 2f;
    [SerializeField]
    private float _speedFloatWarmupDec = 5f;

    [Header("Sprite Fading")]
    [SerializeField]
    private Color _selectColor;
    [SerializeField]
    private Color _deselectColor;


    [Header("Collision Detectors")]
    [SerializeField]
    private EdgeCollider2D _groundCollider;
    [SerializeField]
    private EdgeCollider2D _leftCollider;
    [SerializeField]
    private EdgeCollider2D _rightCollider;

#pragma warning restore 0649

    private Rigidbody2D _rb2d;
    private float animatorSpeedFloat = 0;
    HashSet<Collider2D> _characterColliders;
    public bool IsSelected { get; set; }
    HashSet<SpriteRenderer> _rigSpriteRenderers;
    private Vector2 _movement;
    private float _initialMass;
    private BoxCollider2D _mainCollider;
    private float _initialMainColliderEdgeRadius;

    public bool IsGrounded => RaycastCollider(_groundCollider, _groundDetectionMask);
    public bool FacingRight { get; set; }

    public bool RaycastCollider(EdgeCollider2D collider, LayerMask raycastMask)
    {
        var point1 = collider.points[0];
        var point2 = collider.points[1];
        point1.Scale(transform.localScale);
        point2.Scale(transform.localScale);
        var startPoint = (Vector2)transform.position + point1;
        var raycastVector = point2 - point1;
        Debug.DrawLine(startPoint, startPoint + raycastVector);
        var result = Physics2D.Raycast(startPoint, raycastVector.normalized, raycastVector.magnitude, raycastMask);
        return result;
    }

    void Awake()
    {
        _characterColliders = new HashSet<Collider2D>(GetComponentsInChildren<Collider2D>());
        _rb2d = GetComponent<Rigidbody2D>();
        if (_rigAnimator == null)
            _rigAnimator = GetComponentInChildren<Animator>();
        _rigSpriteRenderers = new HashSet<SpriteRenderer>(_rigAnimator.GetComponentsInChildren<SpriteRenderer>());
        SetEnabledObjects(false);
        _initialMass = _rb2d.mass;
        _mainCollider = GetComponent<BoxCollider2D>();
        _initialMainColliderEdgeRadius = _mainCollider.edgeRadius;
        FacingRight = true;
    }

    private void Update()
    {
        if (_mainCollider.edgeRadius != _initialMainColliderEdgeRadius * transform.localScale.x)
            _mainCollider.edgeRadius = _initialMainColliderEdgeRadius * transform.localScale.x;

        _movement = IsSelected
                            ? new Vector2(Input.GetAxisRaw(_horizontalAxis),
                                   Input.GetAxisRaw(_verticalAxis))
                            : Vector2.zero;
        var rawMovement = _movement;
        if (_movement.x != 0f)
        {
            // Stop if moving against a wall
            var collider = _movement.x > 0f ? _rightCollider : _leftCollider;
            if (RaycastCollider(collider, _sideDetectionMask))
                _movement.x = 0f;
        }

        if (Input.GetButtonDown(_verticalAxis)
            && _movement.y != 0
            && Math.Sign(_movement.y) == Mathf.Sign(_rb2d.gravityScale))
        {
            Jump();
        }
        HandleAnimation(_movement, rawMovement);
    }

    private void FixedUpdate()
    {
        HandleMovement(_movement);
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

    void HandleAnimation(Vector2 direction, Vector2 inputDirection)
    {
        // Animation stuff
        // Run
        var goalFloatValue = direction.x == 0f ? 0f : 1f;
        var animAcc = goalFloatValue == 0f ? _speedFloatWarmupDec : _speedFloatWarmupAcc;
        animatorSpeedFloat = Mathf.MoveTowards(animatorSpeedFloat, goalFloatValue, Time.deltaTime * animAcc);
        _rigAnimator.SetFloat(_speedFloat, animatorSpeedFloat);
        _rigAnimator.SetBool(_runningBool, animatorSpeedFloat > 0f);
        // Flip
        if (inputDirection.x != 0f)
        {
            FacingRight = inputDirection.x > 0f;
            _rigAnimator.SetBool(_turnAroundBool, !FacingRight);
        }
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
            _rb2d.AddForce(Vector2.up * _jumpForce * Mathf.Sign(_rb2d.gravityScale) * (_rb2d.mass / _initialMass));
    }

    /// <summary>
    /// Makes the character interact with all objects nearby.
    /// </summary>
    public void Interact()
    {
        Debug.Log($"{name} interacted.");
        var offset = _interactionBoxOffset;
        if (!FacingRight) offset.x *= -1;
        Vector2 center = ((Vector2)transform.position) + offset;
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

        _rigAnimator.SetTrigger(_interactTrigger);
    }

    void OnDrawGizmos() {
        var offset = _interactionBoxOffset;
        if (!FacingRight) offset.x *= -1;
        Vector2 center = ((Vector2)transform.position) + offset;
        Gizmos.DrawWireCube(center, _interactionBoxSize);
    }

    public virtual void Select()
    {
        IsSelected = true;
        SetEnabledObjects(true);
        SetSpriteSelectedColors(true);
    }

    public virtual void Deselect()
    {
        IsSelected = false;
        SetEnabledObjects(false);
        SetSpriteSelectedColors(false);
    }

    void SetEnabledObjects(bool state)
    {
        foreach (var obj in _enabledOnSelect)
        {
            if (obj == null) continue;
            obj.SetActive(state);
        }
    }

    void SetSpriteSelectedColors(bool selected)
    {
        foreach (var spr in _rigSpriteRenderers)
        {
            spr.color = selected ? _selectColor : _deselectColor;
        }
    }

}