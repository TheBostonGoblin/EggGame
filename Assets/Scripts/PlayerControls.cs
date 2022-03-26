using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // Start is called before the first frame update
    const float moveSpeed = 2.0f;
    const float jumpForce = 7.0f;
    BoxCollider2D playerBoxCollider2D;
    Rigidbody2D rigidBody;
    [SerializeField] Animator animator;
    [SerializeField] private LayerMask groundLayer;
    float horizontalMovement;
    public bool facingRight;
    int extraJumps;
    private bool isJumping;

    void Start()
    {
        extraJumps = 1;
        playerBoxCollider2D = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        facingRight = true;
        rigidBody.freezeRotation = true;
        isJumping = false;
    }
    public void HorizontalMovement()
    {
        //Horizontal Movement
        horizontalMovement = Input.GetAxis("Horizontal");
        transform.position += (new Vector3(horizontalMovement, 0, 0) * moveSpeed) * Time.fixedDeltaTime;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
        
        if (horizontalMovement < 0 && facingRight || horizontalMovement > 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }
    public void VerticalMovement()
    {
        if (IsGrounded())
        {
            extraJumps = 1;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("takeOff");
                rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
            
        }
        else if(!IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0.0f);
                rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                extraJumps--;
            }
        }

    }
    public bool IsGrounded()//checks if the player is touching the ground via raycasting
    {

        /*Vector2 centerOfBoxCollider = playerBoxCollider2D.bounds.center;
        Vector2 direction = Vector2.down;
        float distanceToGround = playerBoxCollider2D.bounds.extents.y;
        float extraHeightForTest = 0.03f;
        Color rayColor = Color.red;
        RaycastHit2D raycastHit = Physics2D.Raycast(centerOfBoxCollider, direction, distanceToGround + extraHeightForTest, groundLayer);
        Debug.DrawRay(centerOfBoxCollider, direction * (distanceToGround + extraHeightForTest), rayColor);*/
        float width = playerBoxCollider2D.bounds.extents.x;
        float height = 0.3f;
        Vector2 size = new Vector2(width,height);
        float positionOfBoxY = -playerBoxCollider2D.bounds.extents.y;
        float positionOfBoxX = playerBoxCollider2D.bounds.center.x;
        Vector2 point = new Vector2(positionOfBoxX,positionOfBoxY);
        Physics2D.OverlapBox(point,size,0.0f,groundLayer.value);

        return Physics2D.OverlapBox(point, size, 0.0f, groundLayer.value);
        ///for this to return true their must be a intercetion between the ray and the ground 
        ///because the box collider can prevent a intercection(distance can be zero and is therefore not interceting)
        ///so the +.1f ensures when the player lands their is a intercetion  

    }
    public bool IsRising()
    {
        if (rigidBody.velocity.y > 0.1f)
        {
            return true;
        }
        return false;
    }
    public bool IsFalling()
    {
        if (rigidBody.velocity.y <= 0.0f)
        {
            return true;
        }
        return false;
    }



    // Update is called once per frame
    private void FixedUpdate()
    {
        animator.SetFloat("VSpeed", Mathf.Abs(rigidBody.velocity.y));
        animator.SetBool("IsGrounded", IsGrounded());
        HorizontalMovement();
    }
    void Update()
    {
        Debug.Log(IsGrounded());
        VerticalMovement();
         
        /*if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("takeOff");
        }
        */
    }
    public void TestMethod()
    {
        print("Testing");
    }
}
