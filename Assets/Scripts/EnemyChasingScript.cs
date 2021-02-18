using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingScript : StateMachineBehaviour
{
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Assign the enemy an player to chase
		GameObject closestPlayer = animator.GetComponent<EnemyData>().CheckIfPlayerEnterRange(animator, animator.GetComponent<EnemyData>().m_detectionRange);
		if (closestPlayer)
		{
			animator.SetBool("IsChasing", true);
			animator.GetComponent<NavMeshAgentScript>().target = closestPlayer.transform;
		}
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Update the enemy's target to chase, if it returns a null (that means no player is in range), return to patrol state;
		GameObject closestPlayer = animator.GetComponent<EnemyData>().CheckIfPlayerEnterRange(animator, animator.GetComponent<EnemyData>().m_detectionRange);
		if (closestPlayer)
		{
			animator.GetComponent<NavMeshAgentScript>().target = closestPlayer.transform;
		}
		else if (!closestPlayer)
		{
			animator.SetBool("IsChasing", false);
		}

		// Check whether is target within attack range
		GameObject target = animator.GetComponent<EnemyData>().CheckIfPlayerEnterRange(animator, animator.GetComponent<EnemyData>().m_attackRange);
		if (target)
		{
			animator.GetComponent<NavMeshAgentScript>().target = target.transform;
			animator.SetBool("IsAttacking", true);
		}
		else if (!target)
		{
			animator.SetBool("IsAttacking", false);
		}
	}

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
