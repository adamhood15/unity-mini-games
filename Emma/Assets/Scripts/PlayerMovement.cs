using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Serialize Field makes the variable visible in the inspector
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityScale;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private bool isFacingRight = true;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;

    private void Awake()
    {
        // Grab references to the components
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");


        // Flip the player's sprite using transform.rotate
        if (horizontalInput > 0 &&isFacingRight == false)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight == true)
        {
            Flip();
        }
        
      
        // Wall Jump Logic
        if(wallJumpCooldown > 0.2f)
        {
       

            // Control the player's movement
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if(onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
            {
                body.gravityScale = 7;
            }

            // Jump
            if(Input.GetKey(KeyCode.Space) && isGrounded())
                Jump();
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }

        // Update the animator
        anim.SetBool("isRunning", horizontalInput != 0);
        anim.SetBool("isGrounded", isGrounded());

        print(onWall());
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            anim.SetTrigger("jump");
        }
        else if(onWall() && !isGrounded())
        {
            wallJumpCooldown = 0;
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * speed, jumpForce);
            // Flip the player's direction
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            // Reset the gravity scale
            body.gravityScale = gravityScale;
            // Trigger the jump animation
            anim.SetTrigger("jump");
        }
        // {
        //     body.velocity = new Vector2(-transform.localScale.x * speed, jumpForce);
        //     wallJumpCooldown = 0;
        //     anim.SetTrigger("jump");
        // }
        // body.velocity = new Vector2(body.velocity.x, jumpForce);
        // anim.SetTrigger("jump");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isGrounded())
        {
         anim.SetBool("jump", false); // Set the jumping parameter to false when grounded

        }
        
    }

    private bool isGrounded()
    {
        // Uses a virtual ray to check if the player is grounded
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
        
    }

     private bool onWall()
    {
        // Uses a virtual ray to check if the player is on a wall
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
        
    }
}
