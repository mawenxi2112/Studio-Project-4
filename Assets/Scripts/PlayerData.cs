using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // Start is called before the first frame update

    public int m_currentHealth;
    public int m_maxHealth;
    public float m_currentMoveSpeed;
    public float m_maxMoveSpeed;
    public bool m_iFrame;
    public double m_iFrameCounter;
    public double m_iFrameThreshold;

    void Start()
    {
        // Initialize base values
        m_maxHealth = 5;
        m_currentHealth = m_maxHealth;
        m_maxMoveSpeed = 5f;
        m_currentMoveSpeed = m_maxMoveSpeed;
        m_iFrame = false;
        m_iFrameCounter = 0f;
        m_iFrameThreshold = 0.2f;
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

}
