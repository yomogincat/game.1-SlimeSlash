using UnityEngine;

public class BeeEnemy : FlyingEnemy
{
    [SerializeField] protected float patrolRadius = 2f;

    [SerializeField] protected float detectRadius = 6f;
    [SerializeField] protected float loseRadius = 8f;

    [SerializeField] protected float arriveDistance = 0.2f;

    [SerializeField] protected float accelerationX = 2f;
    [SerializeField] protected float accelerationY = 1f;

    [SerializeField] protected float chaseAccelerationScale;
    [SerializeField] protected float chaseSpeed;

    [SerializeField] protected float targetUpdateInterval = 0.6f;
    protected float targetUpdateTimer;

    protected Vector2 moveVelocity;
    protected Vector2 patrolCenter;
    protected Vector2 targetPoint;

    protected float playerDistance;
    protected bool isPlayerDetected;
    protected bool isChasing;

    protected override void Start()
    {
        base.Start();
        patrolCenter = transform.position;
    }
    protected override void Move()
    {
        direction = Mathf.Sign(rb.linearVelocity.x);
         
        Vector2 targetVelocity = moveDirection * GetMoveSpeed();
        moveVelocity.x = Mathf.MoveTowards(
            moveVelocity.x ,
            targetVelocity.x,
            accelerationX * GetAcclerationScale() * Time.fixedDeltaTime);

        moveVelocity.y = Mathf.MoveTowards(
            moveVelocity.y,
            targetVelocity.y,
            accelerationY * GetAcclerationScale() * Time.fixedDeltaTime);

        rb.linearVelocity = moveVelocity;
    }
    protected virtual float GetMoveSpeed()
    {
        if (isChasing)
            return chaseSpeed;
        return flySpeed;
    }

    protected virtual float GetAcclerationScale()
    {
        if (isChasing)
            return chaseAccelerationScale;
        return 1;
    }

    protected override void HandleAI()
    {
        playerDistance = Vector2.Distance(transform.position, player.transform.position);
        isPlayerDetected = playerDistance <= detectRadius;

        if (!isChasing && isPlayerDetected)
        {
            isChasing = true;
            targetPoint = player.transform.position;
        }
        if (isChasing)
        {
            if (playerDistance > loseRadius)
                isChasing = false;
        }
        if (Vector2.Distance(transform.position, targetPoint) <= arriveDistance || targetUpdateTimer <= 0)
        {
            UpdateTargetPosition();
        }
        

        moveDirection = (targetPoint - (Vector2)transform.position).normalized;
    }

    protected void UpdateTargetPosition()
    {
        targetUpdateTimer = targetUpdateInterval;
        if (isChasing)
        {
            targetPoint = player.transform.position;
            return;
        }
        
        targetPoint =
            patrolCenter +
            Random.insideUnitCircle * patrolRadius;
    }

    protected override void HandleTimer()
    {
        base.HandleTimer();
        targetUpdateTimer = TickTimer(targetUpdateTimer);

    }

    protected override void HandleAnimation()
    {
        isFocusing = isChasing;
        base.HandleAnimation();
    }
    protected override bool ShouldTurn()
    {
        return false;
    }
}
