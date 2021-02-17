using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolScript : StateMachineBehaviour
{
	private Transform m_targetPos;
	private int newSpot;
	private bool isGoingBack = false;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		newSpot = FindNearestWayPoint(animator);
		m_targetPos = animator.GetComponent<EnemyData>().m_wayPoint[newSpot];
		animator.GetComponent<NavMeshAgentScript>().target = m_targetPos;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (Vector2.Distance(animator.transform.position, m_targetPos.position) > 0.5f)
		{
			// Moving to waypoint using NavMesh Agent
		}
		else
		{
			// If reach waypoint, look for new waypoint to move to
			if (!isGoingBack)
			{
				if (newSpot == animator.GetComponent<EnemyData>().m_wayPoint.Length - 1)
					isGoingBack = true;
				else
					newSpot = newSpot + 1;
			}
			if (isGoingBack)
			{
				if (newSpot == 0)
					isGoingBack = false;
				else
					newSpot = newSpot - 1;
			}

			m_targetPos = animator.GetComponent<EnemyData>().m_wayPoint[newSpot];
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

	public int FindNearestWayPoint(Animator animator)
	{
		// Look for closest waypoint to the enemy and return the id of that waypoint
		float tempdistance = float.MaxValue;
		int closestWaypoint = 0;
		for (int i = 0; i < animator.GetComponent<EnemyData>().m_wayPoint.Length; i++)
		{
			if (Vector2.Distance(animator.transform.position, animator.GetComponent<EnemyData>().m_wayPoint[i].position) < tempdistance)
			{
				closestWaypoint = i;
				tempdistance = Vector2.Distance(animator.transform.position, animator.GetComponent<EnemyData>().m_wayPoint[i].position);
			}
		}

		return closestWaypoint;
	}
}
