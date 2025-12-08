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
    

    private bool canDash = true; //boolean for checking if the player is allowed to dash
    private bool isDashing;//checks if the player is dashing
    public float dashingPower = 10f; //how fast the player can dash
    private float dashingTime = 0.2f;//how long the player can dash
    private float dashingCooldown = 1f;//dash cooldown

    //wall sliding variables
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    //wall jumping variables
    private bool isWallJumping = true;
    private float wallJumpingDirection;
    public float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);


    public float terminalSpeed = 1f;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private Vector3 velocity;

    private float gravity;
    private float jumpVel;

    [SerializeField] private Transform wallCheck;
    public LayerMask wallLayer;
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
        

        if (isDashing)//prevents player from doing any other action while dashing
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

        WallSlide();

        WallJump();

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

    private bool IsWalled()
    {
        
        return Physics2D.OverlapCircle(wallCheck.position, 0.5f, wallLayer);//creates an invisible circle with a radius of 0.5 at the position of the wall check and returns true if it collides with the wall layer

    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && playerInput.x == 1 && playerInput.x == -1)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding) 
        {
            isWallJumping = false;
            wallJumpingDirection = -playerInput.x;
            wallJumpingCounter = wallJumpingTime;
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (playerInput.y < 0 && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
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

    private IEnumerator Dash()//The dash itself
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;//setting the original gravity to zero when the dash happens so its not affected by the dash
        rb.gravityScale = 0f;
        if (playerInput.x == 1)//if the player is facing right, dash to the right
        {
            rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        }
        if (playerInput.x == -1)//same thing for facing left
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
