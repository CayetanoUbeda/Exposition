using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] Camera roomCamera;
    float velocidadAvance = 3.0f;
    public float sensibilidadRaton = 2f;
    float velocidadRotac = 135.0f;
    private bool changeCamera = true;
    void Start()
    {
    	
    }

    // Update is called once per frame
    void Update()
    {
        float avance = Input.GetAxis("Vertical") * velocidadAvance * Time.deltaTime;
	float rotacion = Input.GetAxis("Horizontal") * velocidadRotac * Time.deltaTime;
	transform.Translate(Vector3.forward * avance);
	transform.Rotate(Vector3.up * rotacion);
	if(changeCamera) {
            float mouseY = Input.GetAxis("Mouse Y");
            float mouseX = Input.GetAxis("Mouse X");
            playerCamera.transform.Rotate(-mouseY * sensibilidadRaton, -mouseX * sensibilidadRaton, 0f);
	}
	if(Input.GetKeyDown(KeyCode.C)) {
            if(changeCamera) {
                roomCamera.enabled = true;
                roomCamera.GetComponent<AudioListener>().enabled = true;
                playerCamera.enabled = false;
                playerCamera.GetComponent<AudioListener>().enabled = false;
	    	changeCamera = false;
            } else {
                playerCamera.enabled = true;
                playerCamera.GetComponent<AudioListener>().enabled = true;
                roomCamera.enabled = false;
                roomCamera.GetComponent<AudioListener>().enabled = false;
            	changeCamera = true;
            }
	}
    }
}
