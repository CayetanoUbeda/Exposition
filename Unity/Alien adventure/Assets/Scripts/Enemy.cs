using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform[] wayPoints;
    Vector3 siguientePosicion;
    [SerializeField] float velocidad = 2;
    float distanciaCambio = 0.2f;
    int numeroSiguientePosicion = 0;
    bool reverse = false;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        siguientePosicion = wayPoints[0].position;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, siguientePosicion, velocidad * Time.deltaTime);
        if (Vector3.Distance(transform.position, siguientePosicion) < distanciaCambio) {
            if(reverse) {
                spriteRenderer.flipX = true;
                numeroSiguientePosicion--;
                if (numeroSiguientePosicion < 0) {
                    numeroSiguientePosicion++;
                    reverse = false;
                }
            } else {
                spriteRenderer.flipX = false;
                numeroSiguientePosicion++;
                if (numeroSiguientePosicion >= wayPoints.Length) {
                    numeroSiguientePosicion--;
                    reverse = true;
                }
            }
            siguientePosicion = wayPoints[numeroSiguientePosicion].position;
        }
    }
}
