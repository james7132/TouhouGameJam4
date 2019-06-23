using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAssCrate : MonoBehaviour
{

    [SerializeField]
    private Seija seija;
    [SerializeField]
    private float _xDistanceThreshold = 1.2f;

    private Rigidbody2D _rb2d;

    // Start is called before the first frame update
    void Start()
    {
        if (seija == null)
            seija = GameObject.Find("Seija").GetComponent<Seija>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(seija.transform.position.x - transform.position.x) > _xDistanceThreshold)
            _rb2d.velocity = new Vector2(0f, _rb2d.velocity.y);
    }
}
