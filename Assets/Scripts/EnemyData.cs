using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ENEMY_TYPE
{ 
    MELEE,
    RANGED,
}

public class EnemyData : MonoBehaviourPun
{
    public int m_ID;
    public int m_currentHealth;
    public int m_maxHealth;
    public float m_currentMoveSpeed;
    public float m_maxMoveSpeed;
    public bool m_iFrame;
    public double m_iFrameCounter;
    public double m_iFrameThreshold;
    public float m_weaponRadius;
    public float m_detectionRange;
    public float m_attackRange;
    public float m_rechargeDuration;

    public Transform[] m_wayPoint;
    public ENEMY_TYPE m_type;

    public GameObject[] Player_List;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameObject.GetComponent<NavMeshAgent>().speed = m_maxMoveSpeed;
    }

    void InitializeStat()
    {
        switch (m_type)
        {
        }

    }

    // Update is called once per frame
    void Update()
    {
        Player_List = GameObject.FindGameObjectsWithTag("Player");

        if (m_iFrame)
        {
            // Render a different colour during iFrame

            m_iFrameCounter += Time.deltaTime;

            if (m_iFrameCounter >= m_iFrameThreshold)
            {
                m_iFrame = false;
                m_iFrameCounter = 0f;
            }
        }
    }
    public void SetCurrentHealth(int value)
    {
        m_currentHealth = value;
    }

    public void SetCurrentMoveSpeed(float value)
    {
        m_currentMoveSpeed = value;
    }

    public GameObject CheckIfPlayerEnterRange(Animator animator, float range)
    {
        if (animator.GetComponent<EnemyData>().Player_List.Length <= 0)
        {
            return null;
        }

        float tempdistance = range;
        GameObject closestPlayer = null;
        for (int i = 0; i < animator.GetComponent<EnemyData>().Player_List.Length; i++)
        {
            if (Vector2.Distance(animator.transform.position, animator.GetComponent<EnemyData>().Player_List[i].transform.position) <= tempdistance)
            {
                closestPlayer = animator.GetComponent<EnemyData>().Player_List[i];
                tempdistance = Vector2.Distance(animator.transform.position, animator.GetComponent<EnemyData>().Player_List[i].transform.position);
            }
        }

        return closestPlayer;
    }

}
