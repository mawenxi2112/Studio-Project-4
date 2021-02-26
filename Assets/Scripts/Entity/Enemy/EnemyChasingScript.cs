using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingScript : StateMachineBehaviour
{
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!PhotonNetwork.IsMasterClient)
			return;

		// Assign the enemy an player to chase
		GameObject closestPlayer = animator.GetComponent<EnemyData>().CheckIfPlayerEnterRange(animator, animator.GetComponent<EnemyData>().m_detectionRange);
		if (closestPlayer)
		{
			if (animator.gameObject.GetComponent<EnemyData>().m_type == ENEMY_TYPE.WISP)
			{

			}
			else
			{
				animator.SetBool("IsChasing", true);
				animator.GetComponent<NavMeshAgentScript>().target = closestPlayer.transform;
			}
		}
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

		if (!PhotonNetwork.IsMasterClient)
			return;

		// Update the enemy's target to chase, if it returns a null (that means no player is in range), return to patrol state;
		GameObject closestPlayer = animator.GetComponent<EnemyData>().CheckIfPlayerEnterRange(animator, animator.GetComponent<EnemyData>().m_detectionRange);
		if (closestPlayer)
		{
			animator.GetComponent<NavMeshAgentScript>().target = closestPlayer.transform;
		}
		else if (!closestPlayer)
		{
			if (animator.gameObject.GetComponent<EnemyData>().m_type == ENEMY_TYPE.WISP)
			{

			}
			else
				animator.SetBool("IsChasing", false);
		}

		// TO ENTER ATTACKING STATE
		// Meele - Uses a detection collider on the prefab and another script called EnemyDetectCollider
		// Range - Uses a distance range check
		// Runner - Uses a distance range check
		switch(animator.gameObject.GetComponent<EnemyData>().m_type)
		{
			case ENEMY_TYPE.MELEE:
				break;

			case ENEMY_TYPE.RANGED:
				GameObject RangedPlayerInAttackRange = animator.GetComponent<EnemyData>().CheckIfPlayerEnterRange(animator, animator.GetComponent<EnemyData>().m_attackRange);
				if (RangedPlayerInAttackRange)
				{
					animator.GetComponent<NavMeshAgentScript>().target = RangedPlayerInAttackRange.transform;
					animator.SetBool("IsAttacking", true);
				}
				break;

			case ENEMY_TYPE.RUNNER:
				GameObject RunnerPlayerInAttackRange = animator.GetComponent<EnemyData>().CheckIfPlayerEnterRange(animator, animator.GetComponent<EnemyData>().m_attackRange);
				if (RunnerPlayerInAttackRange)
				{
					animator.GetComponent<NavMeshAgentScript>().target = RunnerPlayerInAttackRange.transform;
					animator.SetBool("IsAttacking", true);
				}
				break;

			case ENEMY_TYPE.WISP:
				break;
		}

	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}
}
