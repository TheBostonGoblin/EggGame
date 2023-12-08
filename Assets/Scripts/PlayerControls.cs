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

        Vector2 centerOfBoxCollider = playerBoxCollider2D.bounds.center;
        Vector2 xAxisExtents = new Vector2(playerBoxCollider2D.bounds.extents.x,0);
        Vector2 rayRightPos = centerOfBoxCollider + xAxisExtents;
        Vector2 rayLeftPos = centerOfBoxCollider - xAxisExtents;
        Vector2 direction = Vector2.down;

        float distanceToGround = playerBoxCollider2D.bounds.extents.y;
        float extraHeightForTest = 0.03f;
        float rayLength = distanceToGround + extraHeightForTest;
        Color rayColor = Color.red;
        RaycastHit2D raycastHitRight = Physics2D.Raycast(rayRightPos, direction, rayLength, groundLayer);
        RaycastHit2D raycastHitLeft = Physics2D.Raycast(rayLeftPos, direction, rayLength, groundLayer);

        Debug.DrawRay(rayRightPos, direction * rayLength, rayColor);
        Debug.DrawRay(rayLeftPos, direction * rayLength, rayColor);

        if (raycastHitRight.collider || raycastHitLeft.collider)
        {
            if (rigidBody.velocity.y == 0)
            {

            }
            return true;
        }
        return false;

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
        if (rigidBody.velocity.y < -0.1f)
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
        animator.SetBool("IsRising", IsRising());
        animator.SetBool("IsFalling",IsFalling());
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
