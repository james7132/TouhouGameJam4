using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{

    [SerializeField]
    private bool isSeija = true;

    [SerializeField]
    private Color seijaColor;
    [SerializeField]
    private Color shinColor;


    void Start()
    {
        foreach (var particles in GetComponentsInChildren<ParticleSystem>())
        {
            particles.startColor = isSeija ? seijaColor : shinColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((isSeija && collision.GetComponent<Seija>() != null)
            || (!isSeija && collision.GetComponent<Shimmyomaru>() != null))
        {
            SceneManager.LoadScene(gameObject.scene.buildIndex + 1);
        }
    }
}
