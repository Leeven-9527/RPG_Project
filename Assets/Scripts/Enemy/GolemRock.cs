using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemRock : MonoBehaviour
{
    public enum RockStates {HitPlayer, HitEnemy, HitNothing}
    public float force;
    public GameObject target;
    public Rigidbody rb;
    public int damage; //石头基础伤害

    private Vector3 direction;
    private RockStates rockStates;
    void Awake() {
        
    }
    void Start()
    {
        // rb = transform.GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    public void FlyToTarget(){
        if(target == null){
            Debug.Log("FlyToTarget--->target == null");
            target = FindObjectOfType<Player>().gameObject;
        }
        Vector3 up = new Vector3(0, 2,0);
        direction = target.transform.position - transform.position + up;
        direction .Normalize();
        // rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(direction * force, ForceMode.Impulse);
        rockStates = RockStates.HitPlayer;
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("enter--->OnCollisionEnter:" + rockStates);
        switch (rockStates)
        {
            case RockStates.HitPlayer:
                Debug.Log("other--->tag:" + other.transform.tag);
                if(other.gameObject.CompareTag("Player")) {
                    Debug.Log(" RockStates.HitPlayer");
                    var agent = other.gameObject.GetComponent<NavMeshAgent>();
                    agent.isStopped = true;
                    agent.velocity = direction * force;

                    var animator = other.gameObject.GetComponent<Animator>();
                    animator.SetTrigger("Dizzy");
                    var characterStats = other.gameObject.GetComponent<CharacterStats>();
                    characterStats.TakeDamage(damage, characterStats);
                    rockStates = RockStates.HitNothing;
                }
            break;
            case RockStates.HitEnemy:

            break;
            case RockStates.HitNothing:

            break;
        }
        
    }
}
