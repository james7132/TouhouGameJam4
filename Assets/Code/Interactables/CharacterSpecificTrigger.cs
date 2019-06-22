using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpecificTrigger : TriggerBehaviour
{
    [Flags]
    public enum SpecificCharacter
    {
        Seija, Shimmyomaru
    }

    static Dictionary<Type, SpecificCharacter> _matchMap = new Dictionary<Type, SpecificCharacter> {
        { typeof(Seija), SpecificCharacter.Seija },
        { typeof(Shimmyomaru), SpecificCharacter.Shimmyomaru },
    };

    #pragma warning disable 0649
    [SerializeField]
    SpecificCharacter _allowedCharacters = (SpecificCharacter)~0;
    #pragma warning restore 0649

    public void Interact(Character source) 
    {
        var character = _matchMap[source.GetType()];
        if ((_allowedCharacters & character) != 0) 
        {
            Invoke(source);
        }
    }
}
