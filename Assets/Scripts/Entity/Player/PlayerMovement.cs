using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody2D rb;
    public Animator animator;
    public Vector2 movement;

    int direction = 2; // Up = 0 Right = 1 Down = 2 Left = 3

    void Start()
    {
        SoundManager.Initialise();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (!GetComponent<PhotonView>())
        {
            if (SceneData.currentScene == "PlayMenu")
            {
                if (SceneData.previousScene == "MainMenu")
                {

                    this.transform.localPosition = new Vector3(-5.0f, -4.0f, 0.0f);

                }
                else if (SceneData.previousScene == "Shop")
                {

                    this.transform.localPosition = new Vector3(6.0f, -0.1f, 0.0f);
                }
                else if(SceneData.previousScene == "GameLobbyScene" || SceneData.previousScene == "Level1Scene")
                {
                    this.transform.localPosition = new Vector3(0.0f, -0.1f, 0.0f);
                }
                else if (SceneData.previousScene == "Tutorial")
                {
                    this.transform.localPosition = new Vector3(-5.6f, -0.1f, 0.0f);
                }
            }
        }

/*        if (GetComponent<PlayerData>().platform == 1)
        {
       
            GetComponent<PlayerData>().m_dashButton = GameObject.Find("Dash").GetComponent<Button>();
            GetComponent<PlayerData>().m_dashButton.onClick.AddListener(Dash);
            Debug.Log("Testing");
        }*/
           
        
    }

    void Update()
    {
        if (GetComponent<PlayerData>().m_isPaused)
            return;
        if (GetComponent<PhotonView>()) // Online
        {
            if (!GetComponent<PhotonView>().IsMine)
                return;

            if (GetComponent<PlayerData>().m_currentHealth <= 0)
            {
                movement.x = 0;
                movement.y = 0;
                return;
            }

            if (GetComponent<PlayerData>().platform == 0) // PC Platform
            {
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    Dash();
                }
            }
            else if (GetComponent<PlayerData>().platform == 1) // Android Platform
            {
                movement = GetComponent<PlayerData>().m_movementJoystick.Direction;

            }

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
            animator.SetFloat("Direction", direction);


            // Checking of current direction
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                // Horizontal is stronger, check for left or right
                if (movement.x < 0)
                    direction = 3;
                else if (movement.x > 0)
                    direction = 1;
            }
            else if (Mathf.Abs(movement.x) < Mathf.Abs(movement.y))
            {
                // Vertical is stronger, check for left or right
                if (movement.y < 0)
                    direction = 2;
                else if (movement.y > 0)
                    direction = 0;
            }
        }
        else if (!GetComponent<PhotonView>()) // Offline
        {
            if (GetComponent<PlayerData>().m_isPaused)
                return;

            if (GetComponent<PlayerData>().platform == 0) // PC Platform
            {
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    Dash();
                }
            }
            else if (GetComponent<PlayerData>().platform == 1) // Android Platform
            {

                movement = GetComponent<PlayerData>().m_movementJoystick.Direction;
 
            }

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
            animator.SetFloat("Direction", direction);

            // Checking of current direction
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                // Horizontal is stronger, check for left or right
                if (movement.x < 0)
                    direction = 3;
                else if (movement.x > 0)
                    direction = 1;
            }
            else if (Mathf.Abs(movement.x) < Mathf.Abs(movement.y))
            {
                // Vertical is stronger, check for left or right
                if (movement.y < 0)
                    direction = 2;
                else if (movement.y > 0)
                    direction = 0;
            }
        }
    }
    
    void FixedUpdate()
    {
        if (!GetComponent<PhotonView>() && GetComponent<PlayerData>().m_isPaused) // If in offline mode and isPaused
            return;

        if (GetComponent<PlayerData>().m_isPaused)
            return;

        rb.AddForce(movement * GetComponent<PlayerData>().m_currentMoveSpeed * Time.fixedDeltaTime);
    }

  public void Dash()
    {
        if (GetComponent<PlayerData>().m_isPaused)
            return;

     
        SoundManager.PlaySound(SOUNDTYPE.FX, SoundManager.SoundName.DASH);
        rb.AddForce(movement.normalized * GetComponent<PlayerData>().m_dashSpeed, ForceMode2D.Impulse);
    }
}
