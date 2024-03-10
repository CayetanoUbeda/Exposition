using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [SerializeField] Transform[] wayPoints;
    Transform siguientePosicion;
    byte numeroSiguientePosicion;
    float distanciaCambio = 0.2f;
    float velocidad = 2;
    float rotacionVelocidad = 5;
    
    void Start()
    {
        siguientePosicion = wayPoints[0];
        numeroSiguientePosicion = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, siguientePosicion.position, velocidad * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, siguientePosicion.rotation, rotacionVelocidad * Time.deltaTime);
        if (Vector3.Distance(transform.position, siguientePosicion.position) < distanciaCambio) {
            numeroSiguientePosicion++;
            if (numeroSiguientePosicion >= wayPoints.Length)
                numeroSiguientePosicion = 0;
            siguientePosicion = wayPoints[numeroSiguientePosicion];
        }
    }
}
