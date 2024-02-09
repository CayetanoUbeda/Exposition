using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//CAYETANO ÚBEDA TOUATI
public class Player : MonoBehaviour
{
    [SerializeField] float velocidad = 5;
    [SerializeField] float velocidadSalto = 7;
    [SerializeField] AudioClip coinSound;
    [SerializeField] AudioClip deathSound;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    SceneController sceneController;
    SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sceneController = FindObjectOfType<SceneController>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if(horizontal != 0) {
            if(Mathf.Abs(rb.velocity.y) < 0.05f) {
                animator.Play("p2_walking");
            } else {
                animator.Play("p2_jump");
            }
            if(horizontal > 0) {
                spriteRenderer.flipX = false;
            } else {
                spriteRenderer.flipX = true;
            }
        } else {
            if(Mathf.Abs(rb.velocity.y) < 0.05f) {
                animator.Play("p2_normal");
            } else {
                animator.Play("p2_jump");
            }
        }
        transform.Translate(horizontal * velocidad * Time.deltaTime, 0, 0);

        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.05f) {
            Vector3 fuerzaSalto = new Vector3(0, velocidadSalto, 0);
            rb.AddForce(fuerzaSalto, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision){
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Void") || other.CompareTag("Enemy") || other.tag == "Enemy") {
            audioSource.clip = deathSound;
            audioSource.Play();
            sceneController.PerderVida();
        }
        if(other.CompareTag("Point")) {
            audioSource.clip = coinSound;
            audioSource.Play();
            sceneController.AnotarItem(other);
        }
        if(other.CompareTag("Portal")) {
            sceneController.AvanzarNivel();
        }
    }
}
