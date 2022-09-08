using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StopAgent : StateMachineBehaviour
{
    //当一个转换开始并且状态机开始计算这个状态时，OnStateEnter被调用
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.GetComponent<NavMeshAgent>().isStopped = true;
    }

    //在OnStateEnter和OnStateExit回调之间的每个更新帧上调用OnStateUpdate
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.GetComponent<NavMeshAgent>().isStopped = true;
    }

    //当转换结束且状态机完成此状态的评估时，OnStateExit将被调用
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.GetComponent<NavMeshAgent>().isStopped = false;
    }

    // OnStateMove在Animator.OnAnimatorMove()之后被调用 
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    //实现处理和影响根运动的代码
    //}

    // OnStateIK在Animator.OnAnimatorIK()之后被调用
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    //实现建立动画IK的代码(逆运动学)
    //}
}
