using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
	[SerializeField] private float velocidadX = 2f;
	[SerializeField] private float velocidadY = 1.6f;
	[SerializeField] Transform prefabExplosion;
	[SerializeField] Sprite sprite_damaged;
	[SerializeField] private bool invert_direction = false;
	[SerializeField] public UnityEngine.UI.Text titlePoints;
	private float fila = 1f;
	private float size_fila = 1.9f;
	private float initial_position;
	private bool can_shoot = true;
	[SerializeField] Transform prefabDisparo;
	void Start()
	{
		StartCoroutine(Disparar());
		initial_position = transform.position.y;
	}
	
	void Update()
	{
		if(invert_direction) {
			if (transform.position.x < -8) {
				if(fila % 2 == 1) {
					if(transform.position.y < (initial_position - (size_fila * fila))) {
						while(transform.position.x < -8) {
							transform.Translate(velocidadX * Time.deltaTime, 0, 0);
						}
						fila += 1;
					} else {
						transform.Translate(0, velocidadY * Time.deltaTime, 0);
					}
				} else {
					transform.Translate(-1 * velocidadX * Time.deltaTime, 0, 0);
				}
			} else if (transform.position.x > 8) {
				if(fila % 2 == 0) {
					if(transform.position.y < (initial_position - (size_fila * fila))) {
						while(transform.position.x > 8) {
							transform.Translate(-1 * velocidadX * Time.deltaTime, 0, 0);
						}
						fila += 1;
					} else {
						transform.Translate(0, velocidadY * Time.deltaTime, 0);
					}
				} else {
					transform.Translate(velocidadX * Time.deltaTime, 0, 0);
				}
			} else if (fila % 2 == 0) {
				transform.Translate(velocidadX * Time.deltaTime, 0, 0);
			} else if (fila % 2 == 1) {
				transform.Translate(-1 * velocidadX * Time.deltaTime, 0, 0);
			}
		} else {
			if (transform.position.x < -8) {
				if(fila % 2 == 0) {
					if(transform.position.y < (initial_position - (size_fila * fila))) {
						while(transform.position.x < -8) {
							transform.Translate(velocidadX * Time.deltaTime, 0, 0);
						}
						fila += 1;
					} else {
						transform.Translate(0, velocidadY * Time.deltaTime, 0);
					}
				} else {
					transform.Translate(-1 * velocidadX * Time.deltaTime, 0, 0);
				}
			} else if (transform.position.x > 8) {
				if(fila % 2 == 1) {
					if(transform.position.y < (initial_position - (size_fila * fila))) {
						while(transform.position.x > 8) {
							transform.Translate(-1 * velocidadX * Time.deltaTime, 0, 0);
						}
						fila += 1;
					} else {
						transform.Translate(0, velocidadY * Time.deltaTime, 0);
					}
				} else {
					transform.Translate(velocidadX * Time.deltaTime, 0, 0);
				}
			} else if (fila % 2 == 1) {
				transform.Translate(velocidadX * Time.deltaTime, 0, 0);
			} else if (fila % 2 == 0) {
				transform.Translate(-1 * velocidadX * Time.deltaTime, 0, 0);
			}
		}
	}

	public float VelocidadX {
		get {
			return velocidadX;
		}
		set {
			velocidadX = value;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
		if(transform.position.y < 4.1 && transform.position.y > -6.2) {
			if(other.tag == "Laser") {
				Destroy(other.gameObject);
				velocidadX = 0;
				velocidadY = 0;
				Transform explotion = Instantiate(prefabExplosion, new Vector3(transform.position.x, transform.position.y + 0.55f, transform.position.z), Quaternion.Euler(-90f, 0f, 0f));
				SpriteRenderer renderer = GetComponent<SpriteRenderer>();
				GetComponent<AudioSource>().Play();
				renderer.sprite = sprite_damaged;
				can_shoot = false;
				titlePoints.text = "Puntos: " + (int.Parse(titlePoints.text.Split(' ')[1]) + 10);
				Destroy(explotion.gameObject, 1f);
				Destroy(other.gameObject, 1f);
				Destroy(gameObject, 1f);
				if(int.Parse(titlePoints.text.Split(' ')[1]) == 140) {
					SceneManager.LoadScene("Nivel2");
				}
			}
		}
        
    }

	IEnumerator Disparar()
	{
		float pausa = Random.Range(3.0f, 7.0f);
		yield return new WaitForSeconds(pausa);
		if(transform.position.y < 4.1 && transform.position.y > -6.2) {
			if(can_shoot){
				Transform disparo = Instantiate(prefabDisparo, transform.position, Quaternion.identity);
			}
		}
		StartCoroutine(Disparar());
	}

	private void OnBecameInvisible()
    {
		if(transform.position.y < -6.2){
			Destroy(gameObject);
		}
    }
}
