using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemtStates { GUARD, PATROL, CHASE, DEAD };//?????????????????

//????????锟斤拷????
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    public EnemtStates enemyStates;
    public float sightRadius; //????????
    public bool isGuard; //??????????
    public float patrolRange; //????锟斤拷
    public float lookAtTime; //??????


    private Animator anim;
    private NavMeshAgent agent;
    private GameObject attackTarget;
    private CharacterStats characterStats;
    private Collider coll;

    private float speed;
    private Vector3 wayPoint;
    private Vector3 guardPos; //??????锟斤拷??
    private float remainLookAtTime;
    private float lastAttackTime;
    private Quaternion guardRotation; //???????????? 


    //??????
    bool isWalk = false;
    bool isChase = false;
    bool isFollow = false;
    bool isDead = false;
    bool isWin = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<Collider>();

        speed = agent.speed;
        guardPos = transform.position;
        remainLookAtTime = lookAtTime;
        guardRotation = transform.rotation;
    }

    void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemtStates.GUARD;
        }
        else
        {
            enemyStates = EnemtStates.PATROL;
            GetNewWayPoint();
        }
    }

    void OnEnable() {
        GameManager.Instance.AddObserver(this);

    }

    void OnDisable() {
        if(!GameManager.IsInitalized()) return;
        GameManager.Instance.RemoveObserver(this);
    }

    //??锟斤拷??????
    void OnFreshEnemtState()
    {
        if (isDead)
        {
            enemyStates = EnemtStates.DEAD;
            return;
        }

        if (isFoundPlayer())
        {
            enemyStates = EnemtStates.CHASE;
            return;
        }
        agent.isStopped = false;
        enemyStates = isGuard ? EnemtStates.GUARD : EnemtStates.PATROL;
    }

    void Update()
    {
        if (characterStats.CurrHealth == 0)
        {
            isDead = true;
        }
        if(!isWin) {
        SwitchStates();
        SwitchAnimation();

        lastAttackTime -= Time.deltaTime;
        }
    }

    void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Critical", characterStats.isCritical);
        anim.SetBool("Death", isDead);
    }
    void SwitchStates()
    {
        OnFreshEnemtState();
        switch (enemyStates)
        {
            case EnemtStates.GUARD:
                EnemyGuardLogic();
                break;

            case EnemtStates.PATROL:
                EnemyPatrolLogic();
                break;

            case EnemtStates.CHASE:
                EnemyChaseLogic();
                break;

            case EnemtStates.DEAD:
                EnemyDeadLogic();
                break;


        }
    }

    bool isFoundPlayer()
    {
        bool isHad = false;
        attackTarget = null;
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                isHad = true;
                break;
            }
        }
        return isHad;
    }

    //Enemy?????
    void EnemyGuardLogic()
    {
        isChase = false;
        if (transform.position != guardPos)
        {
            isWalk = true;
            agent.isStopped = false;
            agent.destination = guardPos;
        }
        //Vector3.SqrMagnitude?锟斤拷???????????????????
        if (Vector3.SqrMagnitude(guardPos - transform.position) < agent.stoppingDistance)
        {
            isWalk = false;
            transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.1f);

        }
    }
    //Enemy??????
    void EnemyChaseLogic()
    {
        agent.speed = speed;
        //???Player
        //??????
        isWalk = false;
        isChase = true;

        if (!isFoundPlayer())
        {
            isFollow = false;
            agent.destination = transform.position;
        }
        else
        {
            isFollow = true;
            agent.isStopped = false;
            agent.destination = attackTarget.transform.position;
        }
        //???????锟斤拷????????
        if (TargetInAttackRange() || TargetInSkillRange())
        {
            isFollow = false;
            agent.isStopped = true;
            if (lastAttackTime < 0)
            {
                lastAttackTime = characterStats.attackData.coolDown;
                //?????锟斤拷?
                characterStats.isCritical = Random.value <= characterStats.attackData.criticalChance;//0.0-1.0
                //??锟斤拷???
                Attack();
            }
        }
    }
    //Enemy??????
    void EnemyPatrolLogic()
    {
        isChase = false;
        isFollow = false;
        agent.speed = speed * 0.5f;

        //?锟斤拷?????????????
        if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
        {
            isWalk = false;
            if (remainLookAtTime > 0)
                remainLookAtTime -= Time.deltaTime;
            else
                GetNewWayPoint();
        }
        else
        {
            isWalk = true;
            agent.destination = wayPoint;
        }
    }
    //Enemt???????
    void EnemyDeadLogic()
    {
        coll.enabled = false;
        agent.enabled = false;

        Destroy(gameObject, 2.0f);
    }
    void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);
        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1))
        {
            wayPoint = hit.position;
        }
        else
        {
            wayPoint = randomPoint;
        }
    }

    //????锟斤拷????
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, patrolRange);
    }

    //?锟斤拷??????????????锟斤拷
    bool TargetInAttackRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.shortRange;
        }
        return false;
    }
    bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.longRanges;
        }
        return false;
    }

    //????
    void Attack()
    {
        if (!attackTarget) return;

        transform.LookAt(attackTarget.transform);
        if (TargetInAttackRange())
        {
            anim.SetTrigger("Attack");
            return;
        }

        if (TargetInSkillRange())
        {
            anim.SetTrigger("Skill");
            return;
        }
    }

    //???????????
    void AttackHitEvent()
    {
        if (attackTarget != null)
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
        }

    }

    /*
     * 游戏结束
     * Enemy获胜动画，停止移动，停止agent
     */
    void IEndGameObserver.EndNotify()
    {
        isWalk = false;
        isChase = false;
        attackTarget = null;
        isWin = true;
        anim.SetBool("Win", isWin);
    }
}
