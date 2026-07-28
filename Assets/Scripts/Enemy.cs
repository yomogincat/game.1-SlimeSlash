using UnityEngine;

public class Enemy : MonoBehaviour
{
    //==================== References ====================

    [Header("References")]
    protected Rigidbody2D rb;

    [SerializeField] protected Animator graphicsAnim;
    [SerializeField] protected Animator eyeAnim;
    [SerializeField] protected AudioSource audioSource;

    //==================== Transforms ====================

    [Header("Transforms")]
    [SerializeField] protected Transform graphics;

    [SerializeField] protected Transform groundRayR;
    [SerializeField] protected Transform groundRayL;
    [SerializeField] protected Transform wallRayR;
    [SerializeField] protected Transform wallRayL;
    [SerializeField] protected Transform playerRay;

    //==================== Layers ====================

    [Header("Layers")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask playerLayer;

    //==================== Sounds ====================

    [Header("Sounds")]
    [SerializeField] protected AudioClip hit;

    //==================== Stats ====================

    [Header("Stats")]
    [SerializeField] protected int maxHealth = 3;
    protected int health;


    //==================== Movement ====================

    [Header("Movement")]
    [SerializeField] protected float walkSpeed = 2f;
    [SerializeField] protected float direction = -1f;

    protected float moveSpeed;

    //==================== Timers ====================

    [Header("Timers")]
    [SerializeField] protected float invincibleTime = 0.2f;

    protected float invincibleTimer;
    protected float knockbackTimer;

    //==================== Checks ====================

    [Header("Checks")]
    [SerializeField] protected float groundRayLength = 0.2f;
    [SerializeField] protected float wallRayLength = 0.2f;
    [SerializeField] protected float playerRayLength = 2f;

    protected RaycastHit2D groundHitR;
    protected RaycastHit2D groundHitL;
    protected RaycastHit2D wallHitR;
    protected RaycastHit2D wallHitL;
    protected RaycastHit2D playerHit;

    protected bool isGroundedR;
    protected bool isGroundedL;

    protected bool isGroundAhead;

    protected bool isTouchingWallR;
    protected bool isTouchingWallL;

    protected bool isTouchingFrontWall;
    protected bool isTouchingBackWall;

    protected bool isPlayerAhead;
    protected bool wasPlayerAhead;

    //==================== Runtime ====================

    //==================== Properties ====================
    public float Direction => direction;

    //==================== State ====================

    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isKnockback;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        health = maxHealth;
    }

    protected virtual void Update()
    {
        HandleGroundCheck();
        HandleWallCheck();
        HandlePlayerCheck();
        HandleTimer();
        HandleMovement();
        HandleSpriteDirection();
        HandleAnimation();
    }

    protected virtual void HandleTimer()
    {
        knockbackTimer = TickTimer(knockbackTimer);
        invincibleTimer = TickTimer(invincibleTimer);
    }

    protected void HandleGroundCheck()
    {
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


    }

    protected void HandleWallCheck()
    {
        wallHitR = Physics2D.Raycast(
            wallRayR.position,
            Vector2.right,
            wallRayLength,
            groundLayer
            );

        wallHitL = Physics2D.Raycast(
            wallRayL.position,
            Vector2.left,
            wallRayLength,
            groundLayer
            );

        isTouchingWallR = wallHitR;
        isTouchingWallL = wallHitL;

        isTouchingFrontWall = direction > 0 ? isTouchingWallR : isTouchingWallL;
        isTouchingBackWall = direction > 0 ? isTouchingWallL : isTouchingWallR;

        isTouchingWall = isTouchingFrontWall || isTouchingBackWall;
    }

    protected void HandlePlayerCheck()
    {
        Vector2 dir = direction > 0 ? Vector2.right : Vector2.left;

        playerHit = Physics2D.Raycast(
            playerRay.position,
            dir,
            playerRayLength,
            playerLayer
            );
        isPlayerAhead = playerHit;
    }
    protected virtual void HandleAnimation()
    {
        if (graphicsAnim == null) return;
        SetAnimValue(graphicsAnim, "YVelocity", rb.linearVelocity.y);
        SetAnimValue(graphicsAnim, "IsGrounded", isGrounded);
    }

    protected virtual void HandleMovement()
    {
        if (isTouchingFrontWall || (!isGroundAhead && isGrounded))
            direction = -direction;

        if (isKnockback)
        {
            isKnockback = knockbackTimer > 0;
            return;
        }
        moveSpeed = walkSpeed * direction;
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }

    protected void HandleSpriteDirection()
    {
        graphics.localScale = new Vector3(direction, 1, 1);
    }

    protected virtual void Die()
    {
        Destroy(gameObject, 0.1f);
    }

    protected float TickTimer(float timer)
    {
        timer = Mathf.Max(timer - Time.deltaTime, 0);
        return timer;
    }
    protected void SetAnimValue<T>(Animator anim, string parameter, T value)
    {
        if (anim == null) return;
        if (value is int i)
            anim.SetInteger(parameter, i);
        else if (value is float f)
            anim.SetFloat(parameter, f);
        else if (value is bool b)
            anim.SetBool(parameter, b);
    }
    protected void SetAnim<T>(string parameter, T value)
    {
        SetAnimValue(graphicsAnim, parameter, value);
        SetAnimValue(eyeAnim, parameter, value);
    }
    public virtual void HitDamage(int damage, Vector2 knockback, float direction, float knockbackTime)
    {
        if (invincibleTimer > 0) return;
        health -= damage;
        isKnockback = true;
        rb.linearVelocity = new Vector2(knockback.x * direction, knockback.y);
        knockbackTimer = knockbackTime;
        invincibleTimer = invincibleTime;
        audioSource.PlayOneShot(hit);
        if (health <= 0) Die();
    }
}