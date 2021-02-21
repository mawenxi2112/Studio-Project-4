using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackingScript : StateMachineBehaviour
{
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		//if (!animator.gameObject.GetComponent<PhotonView>().IsMine)
		//	return;

		if (!PhotonNetwork.IsMasterClient)
			return;

		// WHEN ENTERING ATTACKING STATE
		// Meele - Disable detectCollider
		// Range - Nothing
		// Runner - Disable hitbox collider and enable attack collider
		switch (animator.gameObject.GetComponent<EnemyData>().m_type)
		{
			case ENEMY_TYPE.MELEE:
				animator.transform.Find("DetectCollider").gameObject.GetComponent<PolygonCollider2D>().enabled = false;
				animator.GetComponent<NavMeshAgent>().speed = 0;
				break;

			case ENEMY_TYPE.RANGED:
				animator.GetComponent<NavMeshAgent>().speed = 0;
				break;

			case ENEMY_TYPE.RUNNER:
				animator.transform.Find("Hitbox").GetComponent<CapsuleCollider2D>().enabled = false;
				animator.transform.Find("AttackCollider").GetComponent<CircleCollider2D>().enabled = true;
				animator.GetComponent<NavMeshAgent>().speed = 8;
				break;
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}
}
