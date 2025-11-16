using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float move;
    private float speed = 5f;
    Vector2 boxSize = new Vector2 (1, 1); // setting the box size for the box cast
    public float distance; //Distance of the raycast, set in the inspector 
    public LayerMask groundLayer;
    public enum FacingDirection
    {
        left, right
    }

    void Start()
    {
        
    }

    void Update()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        move = Input.GetAxisRaw("Horizontal");
        transform.Translate(new Vector3(move * speed * Time.deltaTime, 0f, 0f));
    }

    public bool IsWalking()
    {
        if(move == 1 || move == -1)
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
        
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, distance, groundLayer)) //getting access to the physics class and passing the starting point for the raycast
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
        if (move == 1)
        {
            return FacingDirection.right;
        }
        if (move == -1)
        {
            return FacingDirection.left;
        }
        return FacingDirection.left;
    }
}
