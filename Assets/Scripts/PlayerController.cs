using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 playerInput = new Vector2();
    private Rigidbody2D rb;
    private float speed = 5f;
    Vector2 boxSize = new Vector2 (1, 1); // setting the box size for the box cast
    public float distance; //Distance of the raycast, set in the inspector 
    public LayerMask groundLayer;
    public float apexHeight = 3.5f;
    public float apexTime = 0.5f;
    


    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;


    public float terminalSpeed = 1f;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private Vector3 velocity;

    private float gravity;
    private float jumpVel;
    public enum FacingDirection
    {
        left, right
    }

    public enum CharacterState
    {
        Idle, Walking, Jump, Dead
    }

    private CharacterState state = CharacterState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravity = -2 * apexHeight / (apexTime * apexTime);
        jumpVel = 2 * apexHeight / apexTime;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        


        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");
        MovementUpdate(playerInput);
        
        JumpInput(playerInput);
        transform.position += velocity * Time.deltaTime;
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        if (rb.linearVelocity.y > terminalSpeed)
        {
            rb.linearVelocity = new Vector2 (rb.linearVelocity.x, terminalSpeed);
        }

        transform.Translate(new Vector3(playerInput.x * speed * Time.deltaTime, 0f, 0f));
    }

    private void JumpInput(Vector2 playerInput)
    {
        if (coyoteTimeCounter > 0f && playerInput.y == 1)
        {
            velocity.y = jumpVel;
        } else if (!IsGrounded())
        {
            velocity.y += gravity * Time.deltaTime;
            velocity.y = Mathf.Max(velocity.y, -jumpVel);
        } else
        {
            coyoteTimeCounter = 0f;
            velocity.y = 0f;
        }
    }

    public bool IsWalking()
    {
        if(playerInput.x != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
           
    }
    public bool IsGrounded()
    {
        
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, distance, groundLayer)) 
        {
            return true;
        }
        else
        {
            return false;
        } 
    }

    

    public FacingDirection GetFacingDirection()
    {
        if (playerInput.x == 1)
        {
            return FacingDirection.right;
        }
        if (playerInput.x == -1)
        {
            return FacingDirection.left;
        }
        return FacingDirection.right;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (playerInput.x == 1)
        {
            rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        }
        if (playerInput.x == -1)
        {
            rb.linearVelocity = new Vector2(-transform.localScale.x * dashingPower, 0f);
        }
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }
}
