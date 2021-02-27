using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public Animator animator;

    public Vector2 movement;
    public bool isPaused;
    int direction = 2; // Up = 0 Right = 1 Down = 2 Left = 3
    enum PLAYERSPAWNS // This is only relevant for the playMenu
    {
        MAINMENU,
        SINGLEPLAYER,
        MULTIPLAYER,
        SHOP,
        TOTAL
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isPaused = false;
        string prevScene = SceneData.previousScene;
        Debug.Log("prevScene: " + SceneData.previousScene);
        Debug.Log("currentScene: " + SceneData.currentScene);
        if (SceneData.currentScene == "PlayMenu")
        {
            if(prevScene == "MainMenu")
            {
           
                this.transform.localPosition = new Vector3(-5.0f, -4.0f, 0.0f);
                
            }
            else if(prevScene == "Shop")
            {
           
                this.transform.localPosition = new Vector3(6.0f, -0.1f, 0.0f);
            }
        }
    }

    void Update()
    {
        if (isPaused)
            return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Dash
            Debug.Log("Dashed!");
            rb.AddForce(movement.normalized * GetComponent<PlayerData>().m_dashSpeed, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        if (isPaused)
            return;
        rb.AddForce(movement * GetComponent<PlayerData>().m_currentMoveSpeed * Time.fixedDeltaTime);
    }
}
