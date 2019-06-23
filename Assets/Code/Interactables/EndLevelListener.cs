using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelListener : TriggerListener
{
    Color _defaultColor;
    SpriteRenderer _spriteRenderer;

    void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (!_spriteRenderer) return;
        _defaultColor = _spriteRenderer.color;
        SetColor(Color.grey);
    }

    protected override void OnTriggerFired(TriggerBehaviour trigger, Character source) 
    {
        Debug.Log(source);
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex + 1);
    }

    void SetColor(Color color) 
    {
        if (!_spriteRenderer) return;
        _spriteRenderer.color = color;
    }

    protected override void OnEnable() 
    {
        base.OnEnable();
        SetColor(_defaultColor);
    }

    protected override void OnDisable() 
    {
        base.OnDisable();
        SetColor(Color.grey);
    }

}
