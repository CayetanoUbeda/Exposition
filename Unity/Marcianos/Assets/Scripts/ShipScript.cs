using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipScript : MonoBehaviour
{
    [SerializeField] private float velocidad = 5;
    [SerializeField] private int vidas = 3;
    [SerializeField] Transform prefabDisparo;
    [SerializeField] Transform prefabMeteor;
    [SerializeField] Transform prefabExplosion;
    [SerializeField] Sprite sprite_damaged;
    [SerializeField] Sprite sprite_died;
    [SerializeField] public UnityEngine.UI.Text lifeLefts;
    private bool can_shoot = true;
    private bool can_lost = true;
    
    void Start() {
        lifeLefts.text = "Vidas: " + vidas;
        StartCoroutine(throwMeteor());
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if(transform.position.x > 8) {
            transform.Translate(-1 * Time.deltaTime, 0, 0);
        } else if(transform.position.x < -8) {
            transform.Translate(1 * Time.deltaTime, 0, 0);
        } else {
            transform.Translate(horizontal * velocidad * Time.deltaTime, 0, 0);
        }

        if (Input.GetButtonDown("Fire1")){
            StartCoroutine(shoot());
        }   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(can_lost) {
            if(other.tag == "Enemy_Laser" || other.tag == "Enemigo" || other.tag == "Meteor" || other.tag == "Lightning") {
                if(vidas > 1) {
                    StartCoroutine(loseLife(other));
                    vidas -= 1;
                    lifeLefts.text = "Vidas: " + vidas;
                } else {
                    Destroy(other.gameObject);
                    velocidad = 0;
                    SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                    renderer.sprite = sprite_died;
                    GetComponent<AudioSource>().Play();
                    Transform explotion = Instantiate(prefabExplosion, new Vector3(transform.position.x, transform.position.y - 0.55f, transform.position.z), Quaternion.Euler(90f, 0f, 0f));
                    can_shoot = false;
                    lifeLefts.text = "Vidas: 0";
                    Destroy(explotion.gameObject, 1f);
                    Destroy(gameObject, 1f);
                    SceneManager.LoadScene("Lost");
                }
		    } else if (other.tag == "Boss") {
                vidas = 0;
                velocidad = 0;
                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                renderer.sprite = sprite_died;
                GetComponent<AudioSource>().Play();
                Transform explotion = Instantiate(prefabExplosion, new Vector3(transform.position.x, transform.position.y - 0.55f, transform.position.z), Quaternion.Euler(90f, 0f, 0f));
                can_shoot = false;
                lifeLefts.text = "Vidas: 0";
                Destroy(explotion.gameObject, 1f);
                Destroy(gameObject, 1f);
                SceneManager.LoadScene("Lost");
            }
        } else {
            if(other.tag != "Boss") {
                StartCoroutine(loseLife(other));
                vidas -= 1;
                lifeLefts.text = "Vidas: " + vidas;
            } else {
                vidas = 0;
                velocidad = 0;
                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                renderer.sprite = sprite_died;
                GetComponent<AudioSource>().Play();
                Transform explotion = Instantiate(prefabExplosion, new Vector3(transform.position.x, transform.position.y - 0.55f, transform.position.z), Quaternion.Euler(90f, 0f, 0f));
                can_shoot = false;
                lifeLefts.text = "Vidas: 0";
                Destroy(explotion.gameObject, 1f);
                Destroy(gameObject, 1f);
                SceneManager.LoadScene("Lost");
            }
        }
    }

    IEnumerator loseLife(Collider2D other){
        if(can_lost) {
            can_lost = false;
            Destroy(other.gameObject);
            var antigua_velocidad = velocidad;
            velocidad = 0;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            var antiguo_sprite = renderer.sprite;
            renderer.sprite = sprite_damaged;
            GetComponent<AudioSource>().Play();
            Transform explotion = Instantiate(prefabExplosion, new Vector3(transform.position.x, transform.position.y - 0.55f, transform.position.z), Quaternion.Euler(90f, 0f, 0f));
            yield return new WaitForSeconds(1);
            velocidad = antigua_velocidad;
            renderer.sprite = antiguo_sprite;
            Destroy(explotion.gameObject, 1f);
            can_lost = true;
        } else {
            Destroy(other.gameObject);
        }
    }


    IEnumerator shoot() {
        if(can_shoot) {
            Transform disparo = Instantiate(prefabDisparo, transform.position, Quaternion.identity);
            can_shoot = false;
            yield return new WaitForSeconds(2);
            can_shoot = true;
        }
    }

    IEnumerator throwMeteor() {
        Transform meteor = Instantiate(prefabMeteor, new Vector3(Random.Range(-8.7f, 8.7f), 6.7f, 0), Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(1f, 8f));
        StartCoroutine(throwMeteor());
    }
}
