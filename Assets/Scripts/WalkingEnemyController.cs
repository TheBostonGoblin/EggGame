using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyController : MonoBehaviour
{
    
    float walkSpeed;
    Rigidbody2D enemyRB;
    Vector2 enemyMovement;
    bool needToRotate;
    private BoxCollider2D enemyBoxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isFacingRight;


    void Start()
    {
        enemyBoxCollider = GetComponent<BoxCollider2D>();
        enemyRB = GetComponent<Rigidbody2D>();
        walkSpeed = 50f;
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

    public void PatrolEnd()
    {
        PatrolNotAtEdgeRight();
        PatrolNotAtEdgeLeft();
        PatrolHitWallRight();
        PatrolHitWallLeft();
    }
    public void Flip()
    {
        transform.Rotate(Vector2.up, 180);
        isFacingRight = !isFacingRight;
    }
    public bool InAir()
    {
        if (enemyRB.velocity.y > 0.1 || enemyRB.velocity.y < -0.1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void Patrol()
    {
        //if PatrolHitWallLeft() or Right() is false the enemy has not hit a wall keep going
        //if PatrolAtEdgeRight() or Left() is true that means and edge has not been found keep going
        if (!InAir())
        {
            if (isFacingRight)
            {
                enemyRB.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, 0.0f);
                if (PatrolHitWallRight() || !PatrolNotAtEdgeRight())
                {
                    Flip();
                }
            }
            else
            {
                enemyRB.velocity = new Vector2(-walkSpeed * Time.fixedDeltaTime, 0.0f);
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
        Patrol();
    }
    private void FixedUpdate()
    {
        
        
    }

}
