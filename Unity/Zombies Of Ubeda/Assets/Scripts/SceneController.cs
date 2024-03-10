using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject damaged;
    [SerializeField] private GameObject damaged2;  
    [SerializeField] private GameObject newRound;   
    [SerializeField] private GameObject weaponTag;
    [SerializeField] private GameObject enemyPrefab;
    private int zombies_to_kill;
    void Start()
    {
        damaged.SetActive(false);
        damaged2.SetActive(false);
        GameObject.FindGameObjectWithTag("AmmoTag").GetComponent<Text>().text = GameManager.Instance.balas.ToString() + "/" + GameManager.Instance.maxAmmo.ToString();
        newRound.SetActive(false);
        GameObject.FindGameObjectWithTag("LevelTag").GetComponent<Text>().text = "Ronda " + GameManager.Instance.nivel;
        GameObject.FindGameObjectWithTag("PointsTag").GetComponent<Text>().text = "Puntos: " + GameManager.Instance.puntos;
        GameManager.Instance.balas = 17;
        GameManager.Instance.maxAmmo = 17;
        GameManager.Instance.zombies_left = 5 + (GameManager.Instance.nivel * 5);
        StartCoroutine(GameManager.Instance.recuperarVida());
        StartCoroutine(gameStart());
    }

    void Update()
    {
        
    }

    public void PerderVida() {
        if(GameManager.Instance.vidas > 1) {
            GameManager.Instance.vidas -= 1;
            refreshScreen();
        } else {
            StartCoroutine(VolverAlMenuPrincipal());
        }
        
    }

    public void SpawnEnemyInRandomPosition() {
        float minX = 0f;
        float maxX = 0f;
        float minZ = 0f;
        float maxZ = 0f;
        switch(UnityEngine.Random.Range(1,4)) {
            default:
                minX = -8f;
                maxX = 48f;
                minZ = 55f;
                maxZ = 66f;
                break;
            case 2:
                minX = 41f;
                maxX = 48f;
                minZ = 12f;
                maxZ = 68f;
                break;
            case 3:
                minX = -8f;
                maxX = 48f;
                minZ = 12f;
                maxZ = 21f;
                break;
            case 4:
                minX = -8f;
                maxX = 0f;
                minZ = 12f;
                maxZ = 68f;
                break;
        }
        float randomX = UnityEngine.Random.Range(minX, maxX);
        float randomZ = UnityEngine.Random.Range(minZ, maxZ);
        Vector3 spawnPosition = new Vector3(randomX, 0f, randomZ);
        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    public void refreshScreen() {
        if(GameManager.Instance.vidas == 4) {
            damaged.SetActive(false);
            damaged2.SetActive(false);
        } else if(GameManager.Instance.vidas == 3) {
            damaged.SetActive(true);
            damaged2.SetActive(false);
        } else if(GameManager.Instance.vidas == 2) {
            damaged.SetActive(false);
            damaged2.SetActive(true);
        } else if(GameManager.Instance.vidas == 1) {
            damaged.SetActive(true);
            damaged2.SetActive(true);
        }
    }

    public void GastarBala() {
        GameManager.Instance.balas -= 1;
        GameObject.FindGameObjectWithTag("AmmoTag").GetComponent<Text>().text = GameManager.Instance.balas.ToString() + "/" + GameManager.Instance.maxAmmo.ToString();
    }

    private IEnumerator VolverAlMenuPrincipal() {
        Time.timeScale = 0.1f;
        newRound.SetActive(true);
        newRound.GetComponent<Text>().text = "Has perdido!\r\nPuntuación:" + GameManager.Instance.puntos + "\r\nRonda: " + GameManager.Instance.nivel;
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 1;
        GameManager.Instance.puntos = 0;
        GameManager.Instance.vidas = 3;
        GameManager.Instance.balas = 17;
        GameManager.Instance.maxAmmo = 17;
        SceneManager.LoadScene("Menu");
    }

    IEnumerator spawn() {
        if(GameManager.Instance.zombies_left > 0) {
            GameManager.Instance.zombies_left -= 1;
            SpawnEnemyInRandomPosition();
            float seconds_restants = (float)Math.Pow(1.05, 40 - GameManager.Instance.nivel);
            yield return new WaitForSeconds(seconds_restants);
            StartCoroutine(spawn());
        }
    }

    public IEnumerator gameStart() {
        newRound.SetActive(true);
        weaponTag.SetActive(false);
        newRound.GetComponent<Text>().text = "Ronda " + GameManager.Instance.nivel;
        GameManager.Instance.zombies_left = 5 + (GameManager.Instance.nivel * 5);
        zombies_to_kill = GameManager.Instance.zombies_left;
        yield return new WaitForSeconds(10);
        newRound.SetActive(false);
        weaponTag.SetActive(true);
        StartCoroutine(spawn());
    }

    IEnumerator nextLevel() {
        GameManager.Instance.nivel += 1;
        if(GameManager.Instance.nivel < 40) {
            GameManager.Instance.zombies_left = 5 + (GameManager.Instance.nivel * 5);
            zombies_to_kill = GameManager.Instance.zombies_left;
            newRound.SetActive(true);
            weaponTag.SetActive(false);
            newRound.GetComponent<Text>().text = "Ronda " + GameManager.Instance.nivel;
            GameObject.FindGameObjectWithTag("LevelTag").GetComponent<Text>().text = "Ronda " + GameManager.Instance.nivel;
            yield return new WaitForSeconds(15);
            newRound.SetActive(false);
            weaponTag.SetActive(true);
            StartCoroutine(spawn());
        } else {
            
        }  
    }

    public void ZombieKilled() {
        zombies_to_kill -= 1;
        Debug.Log(zombies_to_kill);
        if(zombies_to_kill <= 0) {
            StartCoroutine(nextLevel());
        }
    }
}
