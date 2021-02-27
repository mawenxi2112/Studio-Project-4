using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ENEMY_TYPE
{ 
    MELEE,
    RANGED,
    RUNNER,
    WISP
}

public class EnemyData : MonoBehaviourPunCallbacks, IPunObservable
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

    public GameObject projectilePrefab;

    public GameObject[] Player_List;

    public Animator animator;

    private double changeToRedTimer;
    private Color goColor;

    private float colorA;
    private float colorR;
    private float colorG;
    private float colorB;

    void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.GetComponent<NavMeshAgent>().speed = m_maxMoveSpeed;
        animator.SetInteger("Health", m_currentHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameObject.GetComponent<NavMeshAgent>().speed = m_maxMoveSpeed;
        animator.SetInteger("Health", m_currentHealth);

        goColor.a = goColor.r = goColor.g = goColor.b = colorA = colorR = colorG = colorB = 1.0f;

        changeToRedTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        goColor.a = colorA;
        goColor.r = colorR;
        goColor.g = colorG;
        goColor.b = colorB;

        GetComponent<SpriteRenderer>().color = goColor;

        animator.SetInteger("Health", m_currentHealth);

        Player_List = GameObject.FindGameObjectsWithTag("Player");

        if (m_iFrame)
        {
            // Render a different colour during iFrame

            m_iFrameCounter += Time.deltaTime;
            changeToRedTimer += Time.deltaTime;

            colorG = colorB = (float)changeToRedTimer / ((float)m_iFrameThreshold);

            if (m_iFrameCounter >= m_iFrameThreshold)
            {
                m_iFrame = false;
                m_iFrameCounter = 0f;
                changeToRedTimer = 0f;
                colorA = colorR = colorG = colorB = 1.0f;
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
            if (animator.GetComponent<EnemyData>().Player_List[i].GetComponent<PlayerData>().m_currentHealth <= 0)
                continue;

            if (Vector2.Distance(animator.transform.position, animator.GetComponent<EnemyData>().Player_List[i].transform.position) <= tempdistance)
            {
                closestPlayer = animator.GetComponent<EnemyData>().Player_List[i];
                tempdistance = Vector2.Distance(animator.transform.position, animator.GetComponent<EnemyData>().Player_List[i].transform.position);
            }
        }

        return closestPlayer;
    }

    public void SetAttackColliderEnable(int value)
	{
        if (value == 0)
            gameObject.transform.Find("AttackCollider").gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        else if (value == 1)
            gameObject.transform.Find("AttackCollider").gameObject.GetComponent<PolygonCollider2D>().enabled = true;
    }

    public void SpawnEnemyProjectile()
	{
        if (!PhotonNetwork.IsMasterClient)
            return;

        Vector2 dir = ( new Vector2(gameObject.GetComponent<NavMeshAgentScript>().target.position.x, gameObject.GetComponent<NavMeshAgentScript>().target.position.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized;
        GameObject projectiletmp = PhotonNetwork.Instantiate("EnemyProjectile", new Vector3(0,0,0), Quaternion.identity);
        projectiletmp.GetComponent<Transform>().position = gameObject.transform.Find("ProjectileSpawnPoint").position;
        projectiletmp.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        projectiletmp.GetComponent<EnemyProjectileScript>().rb.AddForce(dir * projectiletmp.GetComponent<EnemyProjectileScript>().moveSpeed, ForceMode2D.Impulse);
    }

    // Syncing of values over the lobby
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // If this current stream is our player
            stream.SendNext(m_currentHealth);
            stream.SendNext(colorA);
            stream.SendNext(colorR);
            stream.SendNext(colorG);
            stream.SendNext(colorB);
        }
        else
        {
            // Network player
            m_currentHealth = (int)stream.ReceiveNext();
            colorA = (float)stream.ReceiveNext();
            colorR = (float)stream.ReceiveNext();
            colorG = (float)stream.ReceiveNext();
            colorB = (float)stream.ReceiveNext();
        }
    }

    public void Despawn()
	{
        Destroy(gameObject);
	}

    public void EnableHitbox()
	{
        gameObject.transform.Find("Hitbox").GetComponent<PolygonCollider2D>().enabled = true;
	}

    public void PlaySound()
    {
        if (m_type == ENEMY_TYPE.MELEE)
            SoundManager.PlaySound(SOUNDTYPE.FX, SoundManager.SoundName.MELEE);
        else if (m_type == ENEMY_TYPE.RANGED)
            SoundManager.PlaySound(SOUNDTYPE.FX, SoundManager.SoundName.BOW);
        else if (m_type == ENEMY_TYPE.RUNNER)
            SoundManager.PlaySound(SOUNDTYPE.FX, SoundManager.SoundName.RUNNER);
    }
}
