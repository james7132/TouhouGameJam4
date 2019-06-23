using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField]
    string _changeCharactersButton = "ChangeCharacters";
    [SerializeField]
    string _interactButton = "Interact";
    [SerializeField]
    string _resetlevelButton = "ResetLevel";
    [SerializeField]
    string _skipLevelButton = "SkipLevel";
    [SerializeField]
    List<Character> _controllableCharacters;

    public IReadOnlyCollection<Character> ControllableCharacters => _controllableCharacters;

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
        foreach (var character in _controllableCharacters)
        {
            if (character != CurrentCharacter)
            {
                character.Deselect();
            }
        }
        CurrentCharacter.Select();
    }

    void Update() 
    {
        if (Input.GetButtonDown(_resetlevelButton)) 
        {
            ResetLevel();
        }
        if (Input.GetButtonDown(_skipLevelButton)) 
        {
            SkipLevel();
        }
        if (Input.GetButtonDown(_changeCharactersButton)) 
        {
            ChangeCharacters();
        }
        if (CurrentCharacter == null) return;
        if (Input.GetButtonDown(_interactButton)) 
        {
            CurrentCharacter.Interact();
        }
    }

    public void ChangeCharacters() 
    {
        if (_controllableCharacters.Count <= 0) return;
        CurrentCharacter?.Deselect();
        _currentIndex = (_currentIndex + 1) % _controllableCharacters.Count;
        CurrentCharacter?.Select();
        Debug.Log($"Changed Characters to: {CurrentCharacter}");
    }

    public void SkipLevel() 
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex + 1);
        Debug.Log("Skipped level!");
    }

    public void ResetLevel() 
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
        Debug.Log("Reset level!");
    }

}