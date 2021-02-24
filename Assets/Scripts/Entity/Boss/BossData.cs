using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class BossData : MonoBehaviour
{
    public int m_ID;
    public int m_currentHealth;
    public int m_maxHealth;
    public float m_currentMoveSpeed;
    public float m_maxMoveSpeed;
    public bool m_iFrame;
    public double m_iFrameCounter;
    public double m_iFrameThreshold;

    public float m_rechargeDuration;

    public GameObject BossBoundary;
    public GameObject TeleportBoundary;
    public int TeleportRangeCheck = 10;
    public double m_teleportCooldown = 15;
    public double m_teleportTickdown = 15;

    public bool m_summonOnce = true;
    public int m_summonCount = 5;

    public Vector3 originalPosition;

    public GameObject[] Player_List;
    public List<GameObject> Player_In_TeleportRange;

    public GameObject DetectCollider;
    public GameObject AttackColliderOne;
    public GameObject AttackColliderTwo;

    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.GetComponent<NavMeshAgent>().speed = m_maxMoveSpeed;
        animator.SetInteger("Health", m_currentHealth);
        Player_In_TeleportRange = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameObject.GetComponent<NavMeshAgent>().speed = m_maxMoveSpeed;
        animator.SetInteger("Health", m_currentHealth);
        originalPosition = gameObject.GetComponent<Transform>().position;
        Player_In_TeleportRange = new List<GameObject>();
        DetectCollider = gameObject.transform.Find("DetectCollider").gameObject;
        AttackColliderOne = gameObject.transform.Find("AttackColliderOne").gameObject;
        AttackColliderTwo = gameObject.transform.Find("AttackColliderTwo").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        animator.SetInteger("Health", m_currentHealth);

        Player_List = GameObject.FindGameObjectsWithTag("Player");

        m_teleportTickdown -= Time.deltaTime;

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

    public bool CheckIfPlayerEnterBoundary(bool BorderOrTeleport) // True = Border, False = Teleport Check
	{
        if (Player_List.Length <= 0)
            return false;

        // Check for player within the Enemy Border
        if (BorderOrTeleport == true)
        {
            BoxCollider2D boundaryCollider = BossBoundary.GetComponent<BoxCollider2D>();
            for (int i = 0; i < Player_List.Length; i++)
            {
                if (Player_List[i].transform.position.x > boundaryCollider.bounds.min.x &&
                    Player_List[i].transform.position.x < boundaryCollider.bounds.max.x &&
                    Player_List[i].transform.position.y > boundaryCollider.bounds.min.y &&
                    Player_List[i].transform.position.y < boundaryCollider.bounds.max.y)
                {
                    return true;
                }
            }
        }
        // Check for player within the teleport border (Used to let the reaper know when he can use the teleport skill & sets a list of player within it to teleport near to)
        else if (BorderOrTeleport == false)
		{
            BoxCollider2D boundaryCollider = TeleportBoundary.GetComponent<BoxCollider2D>();
            List<GameObject> PlayerFound = new List<GameObject>();
            for (int i = 0; i < Player_List.Length; i++)
            {
                if (Player_List[i].transform.position.x > boundaryCollider.bounds.min.x &&
                    Player_List[i].transform.position.x < boundaryCollider.bounds.max.x &&
                    Player_List[i].transform.position.y > boundaryCollider.bounds.min.y &&
                    Player_List[i].transform.position.y < boundaryCollider.bounds.max.y)
                {
                    PlayerFound.Add(Player_List[i]);
                }
            }
            if (PlayerFound.Count != 0)
			{
                Player_In_TeleportRange = PlayerFound;
                return true;
			}
        }

        return false;
	}

    public GameObject FindClosestPlayer()
	{
        if (Player_List.Length <= 0)
        {
            return null;
        }

        float tempdistance = float.MaxValue;
        GameObject closestPlayer = null;
        for (int i = 0; i < Player_List.Length; i++)
        {
            if (Vector2.Distance(gameObject.transform.position, Player_List[i].transform.position) <= tempdistance)
            {
                closestPlayer = Player_List[i];
                tempdistance = Vector2.Distance(gameObject.transform.position, Player_List[i].transform.position);
            }
        }

        return closestPlayer;
    }

    public void Teleport()
	{
        BoxCollider2D boundaryCollider = TeleportBoundary.GetComponent<BoxCollider2D>();

        bool found = false;
        while(!found)
		{
            Vector3 PositionPicked = new Vector3(Random.Range(boundaryCollider.bounds.min.x, boundaryCollider.bounds.max.x), Random.Range(boundaryCollider.bounds.min.y, boundaryCollider.bounds.max.y), 0);

            for (int i = 0; i < Player_In_TeleportRange.Count; i++)
            {
                if (Vector2.Distance(PositionPicked, Player_In_TeleportRange[i].transform.position) <= TeleportRangeCheck)
				{
                    gameObject.transform.position = PositionPicked;
                    found = true;
                    break;
				}
            }
        }
    }

    public void SetActivateOfAttackColliderOne(int value)
	{
        if (value == 0)
            AttackColliderOne.GetComponent<PolygonCollider2D>().enabled = false;
        else if (value == 1)
            AttackColliderOne.GetComponent<PolygonCollider2D>().enabled = true;
    }

    public void SetActivateOfAttackColliderTwo(int value)
	{
        if (value == 0)
            AttackColliderTwo.GetComponent<PolygonCollider2D>().enabled = false;
        else if (value == 1)
            AttackColliderTwo.GetComponent<PolygonCollider2D>().enabled = true;
    }

    public void Summon()
	{
        Debug.Log("SUMMON FUNCTION CALLED");
        BoxCollider2D boundaryCollider = TeleportBoundary.GetComponent<BoxCollider2D>();

        for (int i = 0; i < m_summonCount; i++)
		{
            Vector3 PositionPicked = new Vector3(Random.Range(boundaryCollider.bounds.min.x, boundaryCollider.bounds.max.x), Random.Range(boundaryCollider.bounds.min.y, boundaryCollider.bounds.max.y), 0);

            GameObject wisp = PhotonNetwork.Instantiate("Wisp", PositionPicked, Quaternion.identity);
		}
	}

    public void Despawn()
	{
        Destroy(gameObject);
	}
}
