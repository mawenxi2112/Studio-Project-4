using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeadScript : StateMachineBehaviour
{
	public SpriteRenderer spriterender;
	public Color color;
	float alphaedit;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!animator.gameObject.GetComponent<PhotonView>().IsMine)
			return;

		spriterender = animator.gameObject.GetComponent<SpriteRenderer>();
		color = spriterender.color;
		alphaedit = 255;

		animator.GetComponent<NavMeshAgentScript>().target = null;
		animator.GetComponent<NavMeshAgent>().speed = 0;

		animator.SetBool("IsRecharging", false);
		animator.SetBool("IsAttacking", false);
		animator.SetBool("IsChasing", false);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!animator.gameObject.GetComponent<PhotonView>().IsMine)
			return;

		if (color.a > 0)
		{
			alphaedit -= Time.deltaTime * 80;
			color = new Color(1, 1, 1, alphaedit / 255);
			spriterender.color = color;
		}
		if (color.a <= 0)
		{
			Destroy(animator.gameObject);
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}
}
