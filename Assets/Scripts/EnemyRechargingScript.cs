﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRechargingScript : StateMachineBehaviour
{
	public float m_currentCountDown;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		m_currentCountDown = animator.GetComponent<EnemyData>().m_rechargeDuration;
		animator.GetComponent<NavMeshAgentScript>().target = null;
		animator.GetComponent<NavMeshAgent>().speed = 0;

		// WHEN ENTERING RECHARGE STATE
		// Meele - reenable detection collider
		// Range - None
		switch (animator.gameObject.GetComponent<EnemyData>().m_type)
		{
			case ENEMY_TYPE.MELEE:
				animator.transform.Find("DetectCollider").gameObject.GetComponent<PolygonCollider2D>().enabled = true;
				break;

			case ENEMY_TYPE.RANGED:
				break;
		}

		animator.SetBool("IsRecharging", true);
		animator.SetBool("IsAttacking", false);
		animator.SetBool("IsChasing", false);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		m_currentCountDown -= Time.deltaTime;

		if (m_currentCountDown <= 0)
		{
			animator.SetBool("IsRecharging", false);
			animator.GetComponent<NavMeshAgent>().speed = animator.GetComponent<EnemyData>().m_maxMoveSpeed;

			// Check whether is target within attack range
			GameObject target = animator.GetComponent<EnemyData>().CheckIfPlayerEnterRange(animator, animator.GetComponent<EnemyData>().m_attackRange);
			if (target)
			{
				animator.GetComponent<NavMeshAgentScript>().target = target.transform;
				animator.SetBool("IsAttacking", true);
				return;
			}
			else if (!target)
			{
				animator.SetBool("IsAttacking", false);
			}

			// Check if any player enters the enemy detection range
			GameObject closestPlayer = animator.GetComponent<EnemyData>().CheckIfPlayerEnterRange(animator, animator.GetComponent<EnemyData>().m_detectionRange);
			if (closestPlayer)
			{
				animator.GetComponent<NavMeshAgentScript>().target = closestPlayer.transform;
				animator.SetBool("IsChasing", true);
				return;
			}
			else if (!closestPlayer)
			{
				animator.SetBool("IsChasing", false);
			}
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}
}
