using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLaserScript : MonoBehaviour
{
    [SerializeField] private float velocidadDisparo = 4;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, velocidadDisparo, 0);
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

    }
}
