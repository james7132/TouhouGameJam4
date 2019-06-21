using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    List<Character> _controllableCharacters;

    Character _currentCharacter;

    void Start() {
        _currentCharacter = _controllableCharacters[0];
    }

    void Update() {

    }

}
