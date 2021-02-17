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
            agent.SetDestination(target.position);
    }

	void FixedUpdate()
	{
        m_dir = (new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) - m_previousPosition).normalized;

        m_previousPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
	}
}
