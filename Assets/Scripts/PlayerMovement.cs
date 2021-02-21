using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody2D rb;
    public Animator animator;
    public Vector2 movement;
    int direction = 2; // Up = 0 Right = 1 Down = 2 Left = 3

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GetComponent<PlayerData>().m_dashButton.onClick.AddListener(delegate { Dash(); });
    }

    void Update()
    {
        if (!GetComponent<PhotonView>().IsMine)
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
            movement.x = GetComponent<PlayerData>().m_movementJoystick.Horizontal;
            movement.y = GetComponent<PlayerData>().m_movementJoystick.Vertical;              
        }

        movement.x += 0.1f;
        movement.y += 0.1f;

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
    
    void FixedUpdate()
    {
        rb.AddForce(movement * GetComponent<PlayerData>().m_currentMoveSpeed * Time.fixedDeltaTime);
    }

    void Dash()
    {
        rb.AddForce(movement.normalized * GetComponent<PlayerData>().m_dashSpeed, ForceMode2D.Impulse);
    }
}
