using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float _minSize;
    [SerializeField]
    float _maxSize;

    float _zLevel;
    Camera _camera;
    PlayerController _playerController;

    void Start() 
    {
        _zLevel = transform.position.z;
        _camera = GetComponent<Camera>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    void LateUpdate() {
        var aspectRatio = _camera.aspect;

        var sumPos = Vector2.zero;
        var minX = float.PositiveInfinity;
        var maxX= float.NegativeInfinity;

        var minY = float.PositiveInfinity;
        var maxY = float.NegativeInfinity;
        foreach (var character in _playerController.ControllableCharacters) 
        {
            var position = (Vector2)character.transform.position;
            sumPos += position;
            minX = Mathf.Min(minX, position.x);
            maxX = Mathf.Max(maxX, position.x);
            minY = Mathf.Min(minY, position.y);
            maxY = Mathf.Max(maxY, position.y);
        }

        Vector3 center = sumPos / _playerController.ControllableCharacters.Count;
        center.z = _zLevel;
        transform.position = center;

        var diffX = _camera.aspect * Mathf.Abs(maxX - minX);
        var diffY = Mathf.Abs(maxY - minY);
        var diff = Mathf.Max(diffX, diffY);
        _camera.orthographicSize = Mathf.Clamp(diff / 2, _minSize, _maxSize);
    }
}
