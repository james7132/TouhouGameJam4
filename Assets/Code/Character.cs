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
    #pragma warning restore 0649

    HashSet<Collider2D> _characterColliders;

    // TODO(james7132): Properly implement
    public bool IsGrounded => throw new NotImplementedException();

    // Called before an object's first frame.
    void Start() 
    {
        _characterColliders = new HashSet<Collider2D>(GetComponentsInChildren<Collider2D>());
        SetEnabledObjects(false);
    }

    /// <summary>
    /// Moves the character in a particular direction.
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector2 direction) 
    {
        // TODO(james7132): Properly implement
    }

    public void Jump() 
    {
        if (!IsGrounded) return;
        // TODO(james7132): Properly implement
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