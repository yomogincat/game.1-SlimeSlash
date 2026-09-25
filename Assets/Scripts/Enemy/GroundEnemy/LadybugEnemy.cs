using UnityEngine;
using UnityEngine.EventSystems;

public class LadybugEnemy: GroundEnemy
{
    [Header("Fly")]
    [SerializeField] protected float detectRadius = 4f;
    [SerializeField] protected float loseRadius = 5f;
    [SerializeField] protected float flySpeed = 2f;
    [SerializeField] protected float maxFlySpeedY = 0.5f;

    [SerializeField] protected float takeOffSpeed = 5f;
    [SerializeField] protected float takeOffTime = 0.2f;

    [SerializeField] protected float targetUpdateInterval = 0.3f;

    [SerializeField] protected float fallSpeed;

    protected float targetUpdateTimer;
    protected float takeOffTimer;

    bool isChasing;
    bool isTakingOff;

    Vector2 moveDirection;

 
    protected override void Move()
    {
        direction = Mathf.Sign(rb.linearVelocity.x);
        if (isChasing)
        {
            Vector2 velocity = moveDirection * flySpeed;
            velocity.y = Mathf.Clamp(velocity.y, -maxFlySpeedY, maxFlySpeedY);
            rb.linearVelocity = velocity;
            return;
        }
        if (isTakingOff)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, takeOffSpeed);
            return;
        }

        if (rb.linearVelocity.y < 0)
            rb.linearVelocityY = -fallSpeed;

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    protected override float GetGravityScale()
    {
        if (isChasing)
            return 0f;
        return base.GetGravityScale();
    }

    protected override void HandleAI()
    {
        Vector2 playerPosition = player.transform.position;
        float playerDistance = Vector2.Distance(transform.position, playerPosition);
        bool isPlayerDetected = playerDistance <= detectRadius;

        if (!isChasing)
        {
            if (!isTakingOff)
            {
                if (isPlayerDetected)
                {
                    if (isGrounded)
                    {
                        isTakingOff = true;
                        takeOffTimer = takeOffTime;
                    }
                    else
                    {
                        isChasing = true;
                    }
                }
            }
            else
            {
                if (takeOffTimer <= 0)
                {
                    isChasing = true;
                    isTakingOff = false;
                }
            }
        }
        else
        {
            if (playerDistance > loseRadius || isGrounded)
                isChasing = false;

            if (targetUpdateTimer <= 0)
            {
                targetUpdateTimer = targetUpdateInterval;
                moveDirection = (playerPosition - (Vector2)transform.position).normalized;
            }
        }
    }

    protected override void HandleTimer()
    {
        base.HandleTimer();
        targetUpdateTimer = TickTimer(targetUpdateTimer);
        takeOffTimer = TickTimer(takeOffTimer);
    }

    protected override void HandleAnimation()
    {
        isFocusing = isChasing;
        base.HandleAnimation();
    }
}
