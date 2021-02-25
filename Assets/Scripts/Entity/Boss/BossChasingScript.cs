using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChasingScript : StateMachineBehaviour
{
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!PhotonNetwork.IsMasterClient)
			return;

		// Assign the enemy an player to chase
		GameObject closestPlayer = animator.GetComponent<BossData>().FindClosestPlayer();
		if (closestPlayer)
		{
			animator.SetBool("IsChasing", true);
			animator.GetComponent<NavMeshAgentScript>().target = closestPlayer.transform;
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!PhotonNetwork.IsMasterClient)
			return;

		if (animator.gameObject.GetComponent<BossData>().CheckIfPlayerEnterBoundary(true))
		{
			GameObject closestPlayer = animator.GetComponent<BossData>().FindClosestPlayer();
			if (closestPlayer)
			{
				animator.SetBool("IsChasing", true);
				animator.GetComponent<NavMeshAgentScript>().target = closestPlayer.transform;
			}
		}
		else if (!animator.gameObject.GetComponent<BossData>().CheckIfPlayerEnterBoundary(true))
		{
			animator.gameObject.GetComponent<NavMeshAgentScript>().target = null;
			animator.gameObject.GetComponent<NavMeshAgentScript>().HomingTarget = animator.gameObject.GetComponent<BossData>().originalPosition;
			animator.SetBool("IsChasing", false);
			return;
		}

		if (animator.GetBool("UseAttackOne") != true && animator.GetBool("UseAttackTwo") != true)
		{

			if (animator.gameObject.GetComponent<BossData>().m_summonOnce && animator.gameObject.GetComponent<BossData>().m_currentHealth <= animator.gameObject.GetComponent<BossData>().m_maxHealth / 2)
			{
				animator.SetBool("UseSummon", true);
				animator.gameObject.GetComponent<BossData>().m_summonOnce = false;
			}
			else if (animator.gameObject.GetComponent<BossData>().CheckIfPlayerEnterBoundary(false) == true && animator.gameObject.GetComponent<BossData>().m_teleportTickdown <= 0)
			{
				int count = 0;
				for (int i = 0; i < animator.gameObject.GetComponent<BossData>().Player_In_TeleportRange.Count; i++)
				{
					if (Vector2.Distance(animator.gameObject.transform.position, animator.GetComponent<BossData>().Player_List[i].transform.position) >= 10)
					{
						count++;
					}
				}
				if (count == animator.gameObject.GetComponent<BossData>().Player_In_TeleportRange.Count)
				{
					animator.SetBool("UseTeleport", true);
					animator.gameObject.GetComponent<BossData>().m_rechargeDuration = 0.2f;
					animator.gameObject.GetComponent<BossData>().m_teleportTickdown = animator.gameObject.GetComponent<BossData>().m_teleportCooldown;
					return;
				}
			}
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
