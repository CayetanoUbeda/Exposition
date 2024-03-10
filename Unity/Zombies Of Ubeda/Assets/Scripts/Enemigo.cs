using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    private Transform objetivo;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    public List<Collider> colliders;
    private Collider[] run, attack, walk;
    private AnimationClip run_animation, walk_animation, attack_animation, death_animation;
    public AudioClip death, hit;
    private bool attacking;
    SceneController sceneController;
    private int life;
    private bool dying = false;
    void Start() {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        run = new Collider[2];
        walk = new Collider[2];
        attack = new Collider[2];
        attack[0] = colliders[0];
        attack[1] = colliders[1];
        run[0] = colliders[2];
        run[1] = colliders[3];
        walk[0] = colliders[4];
        walk[1] = colliders[5];
        ToggleColliders(run, true);
        attack_animation = GetComponent<Animation>().GetClip("Attack1");
        walk_animation = GetComponent<Animation>().GetClip("Walk");
        run_animation = GetComponent<Animation>().GetClip("Run");
        death_animation = GetComponent<Animation>().GetClip("Death");
        sceneController = FindObjectOfType<SceneController>();
        life = Random.Range(2, 10);
        GetComponent<NavMeshAgent>().speed = Random.Range(4, 10);
        GetComponent<NavMeshAgent>().acceleration = Random.Range(8, 12);
        objetivo = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update() {
        if(dying == false){
            if(navMeshAgent.enabled) {
                navMeshAgent.SetDestination(objetivo.position);
            }

            float distance = Vector3.Distance(transform.position, objetivo.position);
            if(distance < 2.5f) {
                changeToAttack();
                navMeshAgent.enabled = false;
                StartCoroutine(Attack());
            } else {
                changeToRun();
                navMeshAgent.enabled = true;
            }
        }
    }

    void ToggleColliders(Collider[] array, bool active){
        for(int i = 0; i < array.Length; i++) {
            array[i].enabled = active;
        }
    }

    void changeToRun() {
        GetComponent<Animation>().clip = run_animation;
        GetComponent<Animation>().Play();
    } 
    void changeToWalk() {
        GetComponent<Animation>().clip = walk_animation;
        GetComponent<Animation>().Play();
    } 
    void changeToAttack() {
        GetComponent<Animation>().clip = attack_animation;
        GetComponent<Animation>().Play();
    }

    IEnumerator Die() {
        navMeshAgent.enabled = false;
        dying = true;
        GetComponent<Animation>().clip = death_animation;
        GetComponent<Animation>().Play();
        yield return new WaitForSeconds(1f);
        GetComponent<AudioSource>().clip = death;
        GetComponent<AudioSource>().Play();
        sceneController.ZombieKilled();
        Destroy(gameObject);
    }

    public void receiptHit() {
        life -= 1;
        GetComponent<AudioSource>().clip = hit;
        GetComponent<AudioSource>().Play();
        if(life < 0) {
            StartCoroutine(Die());
        }
    }

    IEnumerator Attack() {
        if(!attacking) {
            attacking = true;
            bool attacked = false;
            yield return new WaitForSeconds(0.8f);
            float distance = Vector3.Distance(transform.position, objetivo.position);
            if(distance < 2.5f && !dying) {
                sceneController.PerderVida();
                attacked = true;
            }
            yield return new WaitForSeconds(0.5f);
            distance = Vector3.Distance(transform.position, objetivo.position);
            if(distance < 2.5f && !attacked && !dying) {
                sceneController.PerderVida();
            }
            attacking = false;
        }
    } 
}
