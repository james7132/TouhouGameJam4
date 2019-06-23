using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpecificTrigger : TriggerBehaviour
{
    [Flags]
    public enum SpecificCharacter
    {
        Seija = 1,
        Shimmyomaru = 2
    }

    static Dictionary<Type, SpecificCharacter> _matchMap = new Dictionary<Type, SpecificCharacter> {
        { typeof(Seija), SpecificCharacter.Seija },
        { typeof(Shimmyomaru), SpecificCharacter.Shimmyomaru },
    };

    #pragma warning disable 0649
    [SerializeField]
    SpecificCharacter _allowedCharacters;
    #pragma warning restore 0649

    public override string ToString() => _allowedCharacters.ToString();

    public override void Interact(Character source) 
    {
        var character = _matchMap[source.GetType()];
        Debug.Log((int)_allowedCharacters);
        Debug.Log((int)character);
        if ((_allowedCharacters & character) != 0) 
        {
            Invoke(source);
        }
    }
}
