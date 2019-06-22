using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField]
    string _changeCharactersButton = "ChangeCharacters";
    [SerializeField]
    List<Character> _controllableCharacters;

    int _currentIndex = 0;
    public Character CurrentCharacter 
    {
        get 
        {
            if (_currentIndex >= 0 && _currentIndex < _controllableCharacters.Count) 
            {
                return _controllableCharacters[_currentIndex];
            }
            return null;
        }
    }

    void Start() 
    {
        if (CurrentCharacter == null) return;
        CurrentCharacter.Select();
    }

    void Update() 
    {
        if (Input.GetButtonDown(_changeCharactersButton)) 
        {
            ChangeCharacters();
        }
        if (CurrentCharacter == null) return;
    }

    public void ChangeCharacters() 
    {
        if (_controllableCharacters.Count <= 0) return;
        CurrentCharacter?.Deselect();
        _currentIndex = (_currentIndex + 1) % _controllableCharacters.Count;
        CurrentCharacter?.Select();
        Debug.Log($"Changed Characters to: {CurrentCharacter}");
    }

}