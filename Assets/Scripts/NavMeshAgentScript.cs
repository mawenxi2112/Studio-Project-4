using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentScript : MonoBehaviour
{
    [SerializeField] 
    public Transform target;

    private NavMeshAgent agent;

    public Vector2 m_dir;
    public Vector2 m_previousPosition;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        m_previousPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);

            //m_dir = (new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) - m_previousPosition).normalized;

            //Debug.Log(m_dir);

            //m_previousPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

            //Vector3 tmp = new Vector3(agent.gameObject.GetComponent<Transform>().localScale.x, agent.gameObject.GetComponent<Transform>().localScale.y, agent.gameObject.GetComponent<Transform>().localScale.z);
            //if (m_dir.x < 0)
            //{
            //    agent.gameObject.GetComponent<Transform>().localScale = new Vector3(-tmp.x, tmp.y, tmp.z);
            //    //Debug.Log("LOOK LEFT");
            //}
            //else if (m_dir.x > 0)
            //{
            //    agent.gameObject.GetComponent<Transform>().localScale = new Vector3(tmp.x, tmp.y, tmp.z);
            //    //Debug.Log("LOOK RIGHT");
            //}
        }
    }

	void FixedUpdate()
	{
    }
}
