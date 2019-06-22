using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueObject : MonoBehaviour
{
    static HashSet<int> _existingObjects = new HashSet<int>();

    [SerializeField]
    int _id;

    void Awake() 
    {
        if (_existingObjects.Contains(_id)) 
        {
            DestroyImmediate(gameObject);
            return;
        }
        _existingObjects.Add(_id);
    }

    void Reset() 
    {
        _id = new System.Random().Next();
    }

}
