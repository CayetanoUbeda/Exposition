using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float minimalVelocity = 4f;
    [SerializeField] private float maxVelocity = 8f;
    void Start()
    {   
        float bulletSpeed = Random.Range(minimalVelocity, maxVelocity);
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, -bulletSpeed, 0);
        GetComponent<AudioSource>().Play();
    }

    void Update()
    {
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
