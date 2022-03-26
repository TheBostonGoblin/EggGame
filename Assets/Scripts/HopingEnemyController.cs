using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopingEnemyController : MonoBehaviour
{
    private Vector2 jumpDirection;
    private float jumpForce;
    private float jumpTimer;
    private Rigidbody2D enemyRB;
    private BoxCollider2D enemyBoxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isFacingRight;
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        jumpDirection = new Vector2(1, 1);
        jumpForce = 2f;
        jumpTimer = 2.0f;
        enemyRB = GetComponent<Rigidbody2D>();
        enemyBoxCollider = GetComponent<BoxCollider2D>();
        enemyRB.freezeRotation = true;
        isFacingRight = true;
    }

    public bool PatrolHitWallRight()
    {
        // right wall hit
        Vector2 laserPosition = new Vector2(enemyBoxCollider.bounds.center.x, (enemyBoxCollider.bounds.center.y));
        Vector2 direction = Vector2.right;
        float distanceToGround = enemyBoxCollider.bounds.extents.y;
        float extraHeightForTest = 0.1f;
        Color rayColor = Color.red;
        RaycastHit2D raycastHit = Physics2D.Raycast(laserPosition, direction, distanceToGround + extraHeightForTest, groundLayer);
        Debug.DrawRay(laserPosition, direction * (distanceToGround + extraHeightForTest), rayColor);
        return raycastHit;
    }
    public bool PatrolHitWallLeft()
    {
        Vector2 laserPosition = new Vector2(enemyBoxCollider.bounds.center.x, (enemyBoxCollider.bounds.center.y));
        Vector2 direction = Vector2.left;
        float distanceToGround = enemyBoxCollider.bounds.extents.y;
        float extraHeightForTest = 0.1f;
        Color rayColor = Color.red;
        RaycastHit2D raycastHit = Physics2D.Raycast(laserPosition, direction, distanceToGround + extraHeightForTest, groundLayer);
        Debug.DrawRay(laserPosition, direction * (distanceToGround + extraHeightForTest), rayColor);
        return raycastHit;
    }
    public bool PatrolNotAtEdgeRight()
    {
        Vector2 laserPosition = new Vector2(enemyBoxCollider.bounds.center.x + enemyBoxCollider.bounds.extents.x + 0.08f, (enemyBoxCollider.bounds.center.y));
        Vector2 direction = Vector2.down;
        float distanceToGround = enemyBoxCollider.bounds.extents.y;
        float extraHeightForTest = 0.1f;
        Color rayColor = Color.red;
        RaycastHit2D raycastHit = Physics2D.Raycast(laserPosition, direction, distanceToGround + extraHeightForTest, groundLayer);
        Debug.DrawRay(laserPosition, direction * (distanceToGround + extraHeightForTest), rayColor);
        return raycastHit;
    }
    public bool PatrolNotAtEdgeLeft()
    {
        Vector2 laserPosition = new Vector2(enemyBoxCollider.bounds.center.x - enemyBoxCollider.bounds.extents.x - 0.08f, (enemyBoxCollider.bounds.center.y));
        Vector2 direction = Vector2.down;
        float distanceToGround = enemyBoxCollider.bounds.extents.y;
        float extraHeightForTest = 0.1f;
        Color rayColor = Color.red;
        RaycastHit2D raycastHit = Physics2D.Raycast(laserPosition, direction, distanceToGround + extraHeightForTest, groundLayer);
        Debug.DrawRay(laserPosition, direction * (distanceToGround + extraHeightForTest), rayColor);

        return raycastHit;
    }
    public void Flip()
    {
        transform.Rotate(Vector2.up, 180);
        isFacingRight = !isFacingRight;
        if (isFacingRight)
        {
            jumpDirection = new Vector2(1, 1);
        }
        else
        {
            jumpDirection = new Vector2(-1, 1);
        }
    }
    public bool IsGrounded()
    {
        Vector2 laserPosition = enemyBoxCollider.bounds.center;
        Vector2 laserDirection = Vector2.down;
        float laserLength = enemyBoxCollider.bounds.extents.y + 0.08f;
        RaycastHit2D raycastHit = Physics2D.Raycast(laserPosition, laserDirection, laserLength, groundLayer);
        Debug.DrawRay(laserPosition, laserDirection * laserLength, Color.green);
        return raycastHit;
    }
    void Hop()
    {
        enemyRB.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
    }
    public void HopingPatrol()
    {
        jumpForce = Random.Range(2,5);

        if (IsGrounded() && jumpTimer <= 0)
        {
            Hop();
            jumpTimer = Random.Range(1, 4);
        }


        if (isFacingRight)
        {
            if (IsGrounded() && jumpTimer <= 0)
            {
                Hop();
                jumpTimer = Random.Range(1, 4);
            }
            if (IsGrounded())
            {
                if (PatrolHitWallRight() || !PatrolNotAtEdgeRight())
                {
                    Flip();
                }
            }
        }
        else
        {
            if (IsGrounded() && jumpTimer <= 0)
            {
                Hop();
                jumpTimer = Random.Range(1, 4);
            }
            if (IsGrounded())
            {
                if (PatrolHitWallLeft() || !PatrolNotAtEdgeLeft())
                {
                    Flip();
                }
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        animator.SetBool("OnGround", IsGrounded());
        if (IsGrounded())
        {
            jumpTimer -= Time.fixedDeltaTime;
        }
        HopingPatrol();

        /*Debug.Log(jumpForce);
        Debug.Log(jumpTimer);*/

    }
}
