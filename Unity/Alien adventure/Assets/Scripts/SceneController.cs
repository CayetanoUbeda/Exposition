using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    private int pointsLeft;
    [SerializeField] private GameObject portal;
    [SerializeField] Text textoGameOver;  
    void Start()
    {
        GameObject.FindWithTag("PointsTV").GetComponent<Text>().text = "Points: " + GameManager.Instance.puntos;
        GameObject.FindWithTag("LifesTV").GetComponent<Text>().text = "Vidas: " + GameManager.Instance.vidas;
        GameObject.FindWithTag("LevelTV").GetComponent<Text>().text = "Nivel: " + GameManager.Instance.nivel;
        pointsLeft = GameObject.FindGameObjectsWithTag("Point").Length;
    }

    void Update()
    {
        
    }

    public void AnotarItem(Collider2D other) {
        GameManager.Instance.puntos += 100;
        pointsLeft -= 1;
        Destroy(other.gameObject);
        GameObject.FindWithTag("PointsTV").GetComponent<Text>().text = "Points: " + GameManager.Instance.puntos;
        Debug.Log("Puntos restantes: " + pointsLeft);
        if(pointsLeft == 0) {
            if(SceneManager.GetActiveScene().name == "Level1") {
                portal.SetActive(true);
            } else {
                textoGameOver.text = "Fin de partida\r\n\r\nHas ganado\r\n\r\nPuntos: " + GameManager.Instance.puntos + "\r\n\r\nVidas:" + GameManager.Instance.vidas;
                textoGameOver.GetComponent<Text>().enabled = true;
                StartCoroutine(VolverAlMenuPrincipal());
            }
        }
    }

    public void PerderVida() {
        GameManager.Instance.vidas--;
        GameManager.Instance.puntos -= 50;
        GameObject.FindWithTag("PointsTV").GetComponent<Text>().text = "Points: " + GameManager.Instance.puntos;
        GameObject.FindWithTag("LifesTV").GetComponent<Text>().text = "Vidas: " + GameManager.Instance.vidas;        
        if(GameManager.Instance.vidas <= 0) {
            Destroy(GameObject.FindAnyObjectByType<Player>().gameObject);
            TerminarPartida();
        } else {
            if(SceneManager.GetActiveScene().name == "Level1") {
                FindAnyObjectByType<Player>().transform.position = new Vector2(-5, -3.815f);
            } else {
                FindAnyObjectByType<Player>().transform.position = new Vector2(-5, -3.5f);
            }
        }
    }

    public void AvanzarNivel() {
        GameManager.Instance.nivel = 2;
        SceneManager.LoadScene("Level2");
    }

    private void TerminarPartida() {
        textoGameOver.text = "Fin de partida\r\n\r\nHas perdido\r\n\r\nPuntos: " + GameManager.Instance.puntos;
        textoGameOver.GetComponent<Text>().enabled = true;
        StartCoroutine(VolverAlMenuPrincipal());
    }

    private IEnumerator VolverAlMenuPrincipal() {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 1;
        GameManager.Instance.puntos = 0;
        GameManager.Instance.vidas = 3;
        SceneManager.LoadScene("Menu");
    }
}
