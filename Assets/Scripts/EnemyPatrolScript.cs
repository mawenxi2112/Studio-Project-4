using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolScript : StateMachineBehaviour
{
	private Transform m_targetPos;
	private int randomSpot;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		randomSpot = Random.Range(0, animator.GetComponent<EnemyData>().m_wayPoint.Length - 1);
		m_targetPos = animator.GetComponent<EnemyData>().m_wayPoint[randomSpot];
		animator.GetComponent<NavMeshAgentScript>().target = m_targetPos;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (Vector2.Distance(animator.transform.position, m_targetPos.position) > 0.1)
		{
			// Moving to waypoint using NavMesh Agent
		}
		else
		{
			// If reach waypoint, look for new waypoint to move to
			randomSpot = Random.Range(0, animator.GetComponent<EnemyData>().m_wayPoint.Length - 1);
			m_targetPos = animator.GetComponent<EnemyData>().m_wayPoint[randomSpot];
			animator.GetComponent<NavMeshAgentScript>().target = m_targetPos;
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
