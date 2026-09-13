using UnityEditorInternal;
using UnityEngine;

public class GroundEnemy : Enemy
{
    //==================== Transforms ====================

    [Header("Transforms")]

    [SerializeField] protected Transform groundRayR;
    [SerializeField] protected Transform groundRayL;

    //==================== Movement ====================

    [Header("Movement")]
    [SerializeField] protected float walkSpeed = 2f;

    protected float moveSpeed;

    //==================== Gravity ====================
    [Header("Gravity")]
    [SerializeField] protected float normalGravity = 5f;

    //==================== Checks ====================

    [Header("Checks")]
    [SerializeField] protected float groundRayLength = 0.2f;

    protected RaycastHit2D groundHitR;
    protected RaycastHit2D groundHitL;

    protected bool isGroundAhead;

    protected bool isGroundedR;
    protected bool isGroundedL;

    protected override void Update()
    {
        HandleGroundCheck();
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        HandleGravity();
    }
   
    protected void HandleGroundCheck()
    {
        wasGrounded = isGrounded;
        groundHitR = Physics2D.Raycast(
            groundRayR.position,
            Vector2.down,
            groundRayLength,
            groundLayer
            );

        groundHitL = Physics2D.Raycast(
            groundRayL.position,
            Vector2.down,
            groundRayLength,
            groundLayer
            );

        isGroundedR = groundHitR;
        isGroundedL = groundHitL;
        isGrounded = isGroundedR || isGroundedL;

        isGroundAhead = direction > 0 ? isGroundedR : isGroundedL;

        if (isGrounded && !wasGrounded)
        {
            OnLanded();
        }
    }

    protected virtual void HandleGravity()
    {
        rb.gravityScale = GetGravityScale();
    }

    protected virtual float GetGravityScale()
    {
        return normalGravity;
    }
    protected override void Move()
    {

        moveSpeed = GetMoveSpeed() * direction;

        rb.linearVelocityX = moveSpeed;
    }
    protected virtual float GetMoveSpeed()
    {
        return walkSpeed;
    }

    protected override bool ShouldTurnByObstacle()
    {
        return isTouchingFrontWall || (!isGroundAhead && isGrounded);
    }
}
