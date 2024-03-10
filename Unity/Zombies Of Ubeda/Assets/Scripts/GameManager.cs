using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int puntos = 0;
    public int vidas = 4;
    public int nivel = 1;
    public int zombies_left;
    public int balas;
    public int maxAmmo;
    SceneController sceneController;
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        balas = 17;
        maxAmmo = 17;
        zombies_left = 5 + (nivel * 5);
        sceneController = FindObjectOfType<SceneController>();
        StartCoroutine(recuperarVida());
        StartCoroutine(sceneController.gameStart());
    }

    void Update()
    {
        
    }

    public IEnumerator recuperarVida() {
        if(vidas < 4) {
            int vidas_antes = vidas;
            yield return new WaitForSeconds(4);
            if(vidas_antes == vidas) {
                vidas += 1;
            }
            sceneController.refreshScreen();
        } else {
            yield return new WaitForSeconds(2);
        }
        StartCoroutine(recuperarVida());
    }
}
