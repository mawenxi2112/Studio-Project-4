using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerEnemyDashScript : MonoBehaviour
{
    public Animator animator;
    public float HomingEndDistance;
    public bool DoOnce;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        DoOnce = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RunnerEnemy_Dashing"))
        {
            if (gameObject.GetComponent<NavMeshAgentScript>().target != null)
            {
                if (Vector2.Distance(gameObject.transform.position, gameObject.GetComponent<NavMeshAgentScript>().target.position) < HomingEndDistance && DoOnce)
                {

                    // End of homing, targets player's last position
                    gameObject.GetComponent<NavMeshAgentScript>().HomingTarget = gameObject.GetComponent<NavMeshAgentScript>().target.position;
                    gameObject.GetComponent<NavMeshAgentScript>().target = null;
                    DoOnce = false;
                }
            }

            if (gameObject.GetComponent<NavMeshAgentScript>().target == null)
            {
                if (Vector2.Distance(gameObject.transform.position, gameObject.GetComponent<NavMeshAgentScript>().HomingTarget) < 0.7f)
                {
                    DoOnce = true;
                    gameObject.GetComponent<NavMeshAgentScript>().HomingTarget = new Vector3(0, 0, 0);
                    animator.SetBool("IsAttacking", false);
                }
            }
        }
    }
}
