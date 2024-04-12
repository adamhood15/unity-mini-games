using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //SerializeField makes a variable editable in Unity
   [SerializeField] float speed;
   [SerializeField] float jumpPower;
   [SerializeField] private LayerMask groundLayer;
   [SerializeField] private LayerMask wallLayer;
   [SerializeField] private string level;
   private Rigidbody2D body;
   private Animator anim;
   private BoxCollider2D boxCollider;
   private float wallJumpCooldown;
   float horizontalInput;
   

   private void Awake()
   {
    //Grab references for rigidbody and animator component from object
    body = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    boxCollider = GetComponent<BoxCollider2D>();

   }

   private void Update()
   {
        horizontalInput = Input.GetAxis("Horizontal");
        
        //flip character right and left when moving
        if(horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        
        else if (horizontalInput< -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //Set animator parameters
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());

        //WallJump Logic
        if(wallJumpCooldown > 0.2f)
        {


            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;
            
            if(Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
   }

//Jumping
private void Jump()
{
    if(isGrounded())
    {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        anim.SetTrigger("Jump");
    }
    else if(onWall() && !isGrounded())//The numbers at the end modify the wall jump feel
    {
        if (horizontalInput == 0)
        {
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 2);
            transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        
        else
             body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 4, 8);

        wallJumpCooldown = 0;                                           
        
    }
  
}



private bool isGrounded()
{
    RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
    return raycastHit.collider != null;
}
private bool onWall()
{
    RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
    return raycastHit.collider != null;
}
public bool canAttack()
{
    return horizontalInput == 0 && isGrounded() && !onWall();
}
}

