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

    void Start()
    {
        // Initialize base values
        m_maxHealth = 5;
        m_currentHealth = m_maxHealth;
        m_maxMoveSpeed = 5f;
        m_currentMoveSpeed = m_maxMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
