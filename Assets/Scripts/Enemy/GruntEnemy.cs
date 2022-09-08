using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntEnemy : EnemyController
{
    public float kickForce = 10;

    public void KickOff() {
        Debug.Log("触发击飞");
        if(attackTarget != null) {
            transform.LookAt(attackTarget.transform);

            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();

            NavMeshAgent attackAgent = attackTarget.GetComponent<NavMeshAgent>();
            attackAgent.isStopped = true;
            //击飞
            // attackAgent.velocity = direction * kickForce;
            //眩晕
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
}
