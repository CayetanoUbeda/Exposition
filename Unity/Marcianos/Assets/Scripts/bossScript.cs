using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bossScript : MonoBehaviour
{
    private float speedParameterX = 2;
    private float speetParameterY = 1.1f;
    private float velocidadX = 2;
    private float velocidadY = -1.1f;
    private int pattern = 0;
    [SerializeField] Transform prefabDisparo;
    [SerializeField] Transform prefabExplosion;
    [SerializeField] Transform prefabLightning;
    [SerializeField] Sprite sprite_damaged;
    [SerializeField] Transform background;
    [SerializeField] Transform background_white;
    [SerializeField] public UnityEngine.UI.Text life;
    [SerializeField] private int vida;
    void Start()
    {
        vida = 100;
        StartCoroutine(Disparar());
        life.text = "Vida del boss: " + vida;
        StartCoroutine(superPower());
    }

    // Update is called once per frame
    void Update()
    {
        if(pattern == 0) {
            transform.Translate(velocidadX * Time.deltaTime, velocidadY * Time.deltaTime, 0);
            if (transform.position.x < -6.5) {
                velocidadX = speedParameterX;
            } else if(transform.position.x > 6.5) {
                velocidadX = -speedParameterX;
            }
            if (transform.position.y < -3.7) {
                velocidadY = speetParameterY;
            } else if(transform.position.y > 3.7) {
                velocidadY = -speetParameterY;
            }
        } else {
            if (transform.position.x < -6.5) {
                velocidadX = 1;
            } else if(transform.position.x > 6.5) {
                velocidadX = -1;
            }
            if(transform.position.y < 1) {
                velocidadY = speetParameterY;
                transform.Translate(velocidadX * Time.deltaTime, velocidadY * Time.deltaTime, 0);
            } else {
                float velocityRandom = Random.Range(2f, 4.0f);
                transform.Translate(velocityRandom * velocidadX * Time.deltaTime, 0, 0);
            }
        }

        float movementPattern = Random.Range(0f, 1.0f);
        if(movementPattern > 0.9997) {
            float choosePattern = Random.Range(0f, 1.0f);
            if(choosePattern < 0.6) {
                Debug.Log("Ha tocado: " + movementPattern + " Pattern: " + pattern);
                pattern = 0;
                if(transform.position.y < -3.7) {
                    velocidadY = 1.1f;
                } else {
                    velocidadY = -1.1f;
                }
                if(transform.position.x < -6.5) {
                    velocidadX = 2;
                } else {
                    velocidadX = -2;
                }
            } else {
                Debug.Log("Ha tocado: " + movementPattern + " Pattern: " + pattern);
                pattern = 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser") {
            StartCoroutine(damaged(other));
        }
    }

    IEnumerator Disparar()
	{
		float pausa = Random.Range(3.0f, 7.0f);
		yield return new WaitForSeconds(pausa);
		if(transform.position.y < 4.1 && transform.position.y > -6.2) {
            Transform disparo = Instantiate(prefabDisparo, transform.position, Quaternion.identity);
		}
		StartCoroutine(Disparar());
	}

    IEnumerator damaged(Collider2D other)
	{
        Destroy(other.gameObject);
        Transform explotion = Instantiate(prefabExplosion, new Vector3(transform.position.x, transform.position.y + 0.55f, transform.position.z), Quaternion.Euler(-90f, 0f, 0f));
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().enabled = false;
        renderer.sprite = sprite_damaged;
        if(pattern == 0) {
            vida -= 6;
        } else {
            vida -= 4;
        }
        if(vida <= 0) {
            life.text = "Has ganado!";
            Destroy(gameObject);
            SceneManager.LoadScene("Win");
        } else {
            life.text = "Vida del boss: " + vida;
            yield return new WaitForSeconds(1);
            GetComponent<Animator>().enabled = true;
            Destroy(explotion.gameObject, 1f);
        }
	}

    IEnumerator superPower() {
        yield return new WaitForSeconds(Random.Range(10f, 25f));
        background.GetComponent<SpriteRenderer>().enabled = false;
        background_white.GetComponent<SpriteRenderer>().enabled = true;
        Transform megaThunder = Instantiate(prefabLightning, new Vector3(transform.position.x - 0.5f, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);
        Transform megaThunder2 = Instantiate(prefabLightning, new Vector3(transform.position.x + 0.5f, transform.position.y - 0.2f, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        background_white.GetComponent<SpriteRenderer>().enabled = false;
        background.GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(superPower());
    }
}
