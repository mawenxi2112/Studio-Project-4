using System.Collections;
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

		if (animator.gameObject.GetComponent<EnemyData>().m_type == ENEMY_TYPE.RUNNER)
		{
			animator.transform.Find("Hitbox").GetComponent<CapsuleCollider2D>().enabled = true;
			animator.transform.Find("AttackCollider").GetComponent<CircleCollider2D>().enabled = false;
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

			// Check if any player enters the enemy detection range
			GameObject closestPlayer = animator.GetComponent<EnemyData>().CheckIfPlayerEnterRange(animator, animator.GetComponent<EnemyData>().m_detectionRange);
			if (closestPlayer)
			{
				animator.GetComponent<NavMeshAgentScript>().target = closestPlayer.transform;
				animator.SetBool("IsChasing", true);
			}
			else if (!closestPlayer)
			{
				animator.SetBool("IsChasing", false);
			}

			// Meele - reenable detection collider
			// Range - None
			switch (animator.gameObject.GetComponent<EnemyData>().m_type)
			{
				case ENEMY_TYPE.MELEE:
					animator.transform.Find("DetectCollider").gameObject.GetComponent<PolygonCollider2D>().enabled = true;
					break;

				case ENEMY_TYPE.RANGED:
					break;

				case ENEMY_TYPE.RUNNER:
					break;
			}
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}
}
