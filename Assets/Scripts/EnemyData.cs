using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_TYPE
{ 
    MELEE,
    RANGED,
}


public class EnemyData : MonoBehaviour
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
    public Transform[] m_wayPoint;
    public ENEMY_TYPE m_type;

    // Start is called before the first frame update
    void Start()
    {
        
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
}
