using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackingScript : StateMachineBehaviour
{
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.GetComponent<NavMeshAgent>().speed = 0;

		// WHEN ENTERING ATTACKING STATE
		// Meele - Disable detectCollider
		// Range - Nothing
		switch (animator.gameObject.GetComponent<EnemyData>().m_type)
		{
			case ENEMY_TYPE.MELEE:
				animator.transform.Find("DetectCollider").gameObject.GetComponent<PolygonCollider2D>().enabled = false;
				break;

			case ENEMY_TYPE.RANGED:
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
