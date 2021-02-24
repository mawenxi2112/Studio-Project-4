using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossRechargingScript : StateMachineBehaviour
{
	public float m_currentCountDown;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!PhotonNetwork.IsMasterClient)
			return;

		m_currentCountDown = animator.GetComponent<BossData>().m_rechargeDuration;
		animator.GetComponent<NavMeshAgentScript>().target = null;
		animator.GetComponent<NavMeshAgent>().speed = 0;

		animator.SetBool("IsRecharging", true);
		animator.SetBool("IsChasing", false);
		animator.SetBool("UseAttackOne", false);
		animator.SetBool("UseAttackTwo", false);
		animator.SetBool("UseTeleport", false);
		animator.SetBool("UseSummon", false);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!PhotonNetwork.IsMasterClient)
			return;

		m_currentCountDown -= Time.deltaTime;

		if (m_currentCountDown <= 0)
		{
			animator.SetBool("IsRecharging", false);
			animator.GetComponent<NavMeshAgent>().speed = animator.GetComponent<BossData>().m_maxMoveSpeed;
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that processes and affects root motion
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}
}
