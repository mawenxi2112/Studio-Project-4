using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityBounce : MonoBehaviour
{
    public float m_startTime;
    public float m_previousOffset;
    public float m_currentOffset;
    public int m_bounceNumber;
    public float m_currentBounceVelocity;
    public float m_bounceVelocity;
    public float m_bounceLoss;
    public float m_gravity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartBounce(int numberOfBounces, bool newBounce)
    {
        m_startTime = Time.time;
        m_previousOffset = 0.0f;
        if (newBounce)
            m_currentBounceVelocity = m_bounceVelocity;
        else
            m_currentBounceVelocity *= m_bounceLoss;
        m_bounceNumber = numberOfBounces;
    }

    // Update is called once per frame
    void Update()
    {
        // Only continue to update if there are bounces
        if (m_bounceNumber > 0)
        {
            // Applying a classic parabolic formula: h(t) = vt + (gt ^ 2 / 2)
            float m_elapsedTime = Time.time - m_startTime;
            m_currentOffset = (m_bounceVelocity * m_elapsedTime) + ((m_gravity * m_elapsedTime * m_elapsedTime) / 2);

            // Apply offset to the current transformation of the game object
            // We only apply the offset to the y transformation because we're only simulating the fake y gravity
            // We have to deduct the previous offset because it has to be additive.
            transform.position = new Vector3(transform.position.x, transform.position.y + m_currentOffset - m_previousOffset, transform.position.z);

            m_previousOffset = m_currentOffset;

            if (m_currentOffset <= 0.0f)
                StartBounce(m_bounceNumber - 1, false);
        }

    }
}
