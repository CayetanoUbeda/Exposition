using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Jugador : MonoBehaviour
{
    [SerializeField] Camera camara;
    [SerializeField] GameObject lightShoot;
    [SerializeField] Transform effect;
    [SerializeField] GameObject dado;
    public Transform[] wayPointsRecharge;
    float velocidadRecharge = 8;
    float rotacionVelocidadRecharge = 700;
    private float distanciaCambio = 0.05f;
    private bool recharging = false;
    SceneController sceneController;
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && recharging == false && GameManager.Instance.balas > 0) {
            StartCoroutine(Shoot());
            Debug.Log("Disparando...");
            float distanciaMaxima = 50;
            RaycastHit hit;
            bool impactado = Physics.Raycast(camara.transform.position, camara.transform.forward, out hit, distanciaMaxima);
            if (impactado) {
                StartCoroutine(Impactado(hit));
            }
        } else if(Input.GetButtonDown("Fire1") && recharging == false && GameManager.Instance.balas <= 0){
            GetComponent<AudioSource>().Play();
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            if(recharging == false && GameManager.Instance.balas < 17) {
                Debug.Log("RECARGANDO...");
                StartCoroutine(Recharge(GameObject.FindGameObjectWithTag("ArmaPropia")));
            }
            
        }
    }

    IEnumerator Shoot() {
        sceneController.GastarBala();
        lightShoot.SetActive(true);
        GameObject arma = GameObject.FindGameObjectWithTag("ArmaPropia");
        arma.GetComponent<AudioSource>().Play();
        Quaternion rotacionInicial = arma.transform.localRotation;
        arma.transform.localRotation = Quaternion.Euler(rotacionInicial.eulerAngles.x - 12.103f, rotacionInicial.eulerAngles.y, rotacionInicial.eulerAngles.z);
        effect.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.1f);
        arma.transform.localRotation = rotacionInicial;
        lightShoot.SetActive(false);
    }

    IEnumerator Impactado(RaycastHit hit){
        Debug.Log("Disparo impactado");
        if (hit.collider.CompareTag("Enemigo")) {
            GameManager.Instance.puntos += 10;
            GameObject.FindGameObjectWithTag("PointsTag").GetComponent<Text>().text = "Puntos: " + GameManager.Instance.puntos;
            Debug.Log("Enemigo acertado");
            dado.SetActive(true);
            hit.collider.GetComponent<Enemigo>().receiptHit();
            yield return new WaitForSeconds(0.1f);
            dado.SetActive(false);
        }
    }

    IEnumerator Recharge(GameObject pistol) {
        recharging = true;
        Transform siguientePosicion = wayPointsRecharge[0];
        int numeroSiguientePosicion = 0;

        while(numeroSiguientePosicion != 7) {
            pistol.transform.position = Vector3.MoveTowards(pistol.transform.position, siguientePosicion.position, velocidadRecharge * Time.deltaTime);
            pistol.transform.rotation = Quaternion.RotateTowards(pistol.transform.rotation, siguientePosicion.rotation, rotacionVelocidadRecharge * Time.deltaTime);
            if (Vector3.Distance(pistol.transform.position, siguientePosicion.position) < distanciaCambio) {
                numeroSiguientePosicion++;
                if (numeroSiguientePosicion >= wayPointsRecharge.Length)
                    break;
                siguientePosicion = wayPointsRecharge[numeroSiguientePosicion];
            }
            yield return new WaitForSeconds(0.1f);
        }
        GameManager.Instance.balas = 17;
        recharging = false;
        GameObject.FindGameObjectWithTag("AmmoTag").GetComponent<Text>().text = GameManager.Instance.balas.ToString() + "/" + GameManager.Instance.maxAmmo.ToString();
    }
}
