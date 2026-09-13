using UnityEngine;

public class FlyingEnemy : Enemy
{
    protected Vector2 moveDirection;
    [SerializeField] protected float flySpeed;
    protected override void Awake()
    {
        base.Awake();

        rb.gravityScale = 0f;
    }

    protected override bool ShouldTurnByObstacle()
    {
        return isTouchingFrontWall;
    }

}