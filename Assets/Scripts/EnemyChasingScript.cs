﻿using System.Collections;
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

		// TO ENTER ATTACKING STATE
		// Meele - Uses a detection collider on the prefab and another script called EnemyDetectCollider
		// Range - Uses a distance range check
		switch(animator.gameObject.GetComponent<EnemyData>().m_type)
		{
			case ENEMY_TYPE.MELEE:
				break;

			case ENEMY_TYPE.RANGED:
				GameObject PlayerInAttackRange = animator.GetComponent<EnemyData>().CheckIfPlayerEnterRange(animator, animator.GetComponent<EnemyData>().m_attackRange);
				if (PlayerInAttackRange)
				{
					animator.GetComponent<NavMeshAgentScript>().target = PlayerInAttackRange.transform;
					animator.SetBool("IsAttacking", true);
				}
				break;
		}

	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}
}
