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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravity = -2 * apexHeight / (apexTime * apexTime);
        jumpVel = 2 * apexHeight / apexTime;
    }

    void Update()
    {
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
        return FacingDirection.left;
    }
}
