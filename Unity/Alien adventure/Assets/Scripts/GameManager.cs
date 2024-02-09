using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int puntos = 0;
    public int vidas = 3;
    public int nivel = 1;
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
        
    }

    void Update()
    {
        
    }
}
