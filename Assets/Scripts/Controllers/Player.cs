using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public Rigidbody rd;

    private CharacterStats characterStats;
    private NavMeshAgent agent;
    private Animator anim;
    private GameObject attackTarget;
    private float lastAttackTime = 0;
    private float stopDistance;

    bool isDead = false;
    private void Awake()
    {
        // GetComponent<组件名称>() //获取组件
        // rd = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();

        stopDistance = agent.stoppingDistance;
    }
    void Start()
    {
        //rd.AddForce(Vector3.up, ForceMode.Force);
        MouseManager.Instance.OnMouseClick += MoveToTarget;
        MouseManager.Instance.OnEnemyClick += EventAttack;

        GameManager.Instance.RigisterPlayer(characterStats);
    }

    // Update is called once per frame
    void Update()
    {
        // rd.AddForce(Vector3.right, ForceMode.Force);
        //float h = Input.GetAxis("Horizontal"); //按键控制左右运动（左）
        //float v = Input.GetAxis("Vertical");//按键控上下运动 （右）AD/<>键
        //rd.AddForce(new Vector3(h * 3 * Time.deltaTime, 0, v * 3 * Time.deltaTime));
        isDead = characterStats.CurrHealth == 0;
        if(isDead) {
            GameManager.Instance.NotifyObserver();
        }
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    private void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude); //Vec3.sqrMagnitude 转换成浮点型的数值
        anim.SetBool("Death", isDead);
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        if(isDead) return;
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;
    }

    private void EventAttack(GameObject obj)
    {
        if(isDead) return;
        if (!obj) return;

        attackTarget = obj;
        characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
        StartCoroutine(MoveToAttackTarget());
    }
    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStats.attackData.shortRange;
        transform.LookAt(attackTarget.transform);
        Vector3 tagPoint = attackTarget.transform.position;
        float attackDistance = characterStats.attackData.shortRange;
        while (Vector3.Distance(tagPoint, transform.position) > attackDistance)
        {
            agent.destination = tagPoint;
            yield return null;
        }

        agent.isStopped = true;
        //Attack
        if(lastAttackTime <= 0)
        {
            bool isBaoji = characterStats.isCritical;
            anim.SetBool("Critical", isBaoji);
            anim.SetTrigger("Attack");
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }

    //攻击动画事件

    void AttackHitEvent()
    {
        var targetStats = attackTarget.GetComponent<CharacterStats>();
        targetStats.TakeDamage(characterStats, targetStats);
    }
}
