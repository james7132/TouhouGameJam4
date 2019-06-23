using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryScene : MonoBehaviour
{
    [SerializeField]
    private StorySlide[] _slides;
    [SerializeField]
    private bool toTitle;

    [System.Serializable]
    public class StorySlide
    {
        [SerializeField]
        private Sprite _sprite;
        public Sprite Sprite => _sprite;
        [SerializeField]
        [Multiline]
        private string _text;
        public string Text => _text;
    }

    int _currentIndex = 0;
    SpriteRenderer _spr;
    TextMeshPro _tmp;

    private void Start()
    {
        _spr = GetComponentInChildren<SpriteRenderer>();
        _tmp = GetComponentInChildren<TextMeshPro>();
        AdvancePage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            AdvancePage();
        }
    }

    void AdvancePage()
    {
        if (_currentIndex < _slides.Length)
        {
            _spr.sprite = _slides[_currentIndex].Sprite;
            _tmp.text = _slides[_currentIndex].Text;
        }
        else if (!toTitle)
            SceneManager.LoadScene(gameObject.scene.buildIndex + 1);
        else
            SceneManager.LoadScene(0);

        _currentIndex++;
    }
}
