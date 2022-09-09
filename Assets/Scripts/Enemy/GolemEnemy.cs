using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemEnemy : EnemyController
{
    public float kickForce = 15;
    public GameObject rockPrefab;
    public Transform handPos;

    public void KickOff()
    {
        if (attackTarget != null && transform.isFacingTarget(attackTarget.transform))
        {
            NavMeshAgent attackAgent = attackTarget.GetComponent<NavMeshAgent>();
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            transform.LookAt(attackTarget.transform);
            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();
            attackAgent.isStopped = true;
             //击飞
            attackAgent.velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    //Animation Event
    public void ThrowRock(){
        if(attackTarget != null) {
            GameObject rock = RockPool.Instance.GetPooledObj();
            rock.transform.position = handPos.position;
            rock.transform.rotation = Quaternion.identity;
            rock.SetActive(true);
            RockPool.Instance.CollectObject(rock, 5.0f);
            // var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
            GolemRock golemRock = rock.GetComponent<GolemRock>();
            golemRock.target = attackTarget;
            golemRock.FlyToTarget();
        }
    }
}
