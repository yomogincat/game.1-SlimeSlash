using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class Enemy : MonoBehaviour
{
    //==================== References ====================

    [Header("References")]
    protected Rigidbody2D rb;

    [SerializeField] protected Animator graphicsAnim;
    [SerializeField] protected Animator[] eyeAnim;
    protected AudioSource audioSource;

    protected Player player;

    //==================== Transforms ====================

    [Header("Transforms")]
    [SerializeField] protected Transform graphics;

    [SerializeField] protected Transform wallRayR;
    [SerializeField] protected Transform wallRayL;
    [SerializeField] protected Transform playerRay;

    //==================== Layers ====================

    [Header("Layers")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask playerLayer;

    //==================== Sounds ====================

    [Header("Sounds")]
    [SerializeField] protected AudioClip hitSE;

    //==================== Stats ====================

    [Header("Stats")]
    [SerializeField] protected int maxHealth = 3;
    protected int health;

    //==================== Attack ====================
    [Header("Attack")]
    [SerializeField] int damage = 1;
    [SerializeField] Vector2 knockbackPower = new Vector2(5, 5);
    [SerializeField] float knockbackTime = 0.1f;
   

    
    //==================== Timers ====================

    [Header("Timers")]
    [SerializeField] protected float invincibleTime = 0.2f;

    protected float invincibleTimer;
    protected float knockbackTimer;

    //==================== Checks ====================

    [Header("Checks")]
    [SerializeField] protected float wallRayLength = 0.2f;
    [SerializeField] protected float playerRayLength = 2f;

   
    protected RaycastHit2D wallHitR;
    protected RaycastHit2D wallHitL;

    protected bool isTouchingWallR;
    protected bool isTouchingWallL;

    protected bool isTouchingFrontWall;
    protected bool isTouchingBackWall;

    protected bool isPlayerAhead;
    protected bool wasPlayerAhead;

    protected bool isPlayerBehind;
    protected bool wasPlayerBehind;

    //==================== Runtime ====================


    //==================== Events ====================
    public event Action OnDie;
    //==================== Properties ====================
    public float Direction => direction;

    //==================== State ====================
    [Header("State")]
    [SerializeField] protected float direction = -1f;

    protected bool isGrounded;
    protected bool wasGrounded;
    protected bool isTouchingWall;
    protected bool isFocusing;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        health = maxHealth;
        player = FindFirstObjectByType<Player>();
    }

    protected virtual void Update()
    {
        HandleDebug();
        HandleWallCheck();
        HandlePlayerCheck();
        HandleAI();
        HandleTimer();
        HandleSpriteDirection();
        HandleAnimation();
    }

    protected virtual void FixedUpdate()
    {
        HandleMovement();
    }

    protected virtual void HandleDebug()
    {

    }
    protected virtual void HandleTimer()
    {
        knockbackTimer = TickTimer(knockbackTimer);
        invincibleTimer = TickTimer(invincibleTimer);
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

    protected virtual void HandlePlayerCheck()
    {
        PlayerFrontCheck();
    }
    protected virtual void HandleAnimation()
    {
        SetAnim("YVelocity", rb.linearVelocity.y);
        SetAnim("IsGrounded", isGrounded);
        SetAnim("IsFocusing", isFocusing);
    }

   

    protected void HandleSpriteDirection()
    {
        graphics.localScale = new Vector3(direction, 1, 1);
    }

    protected virtual void HandleAI()
    {

    }
    protected virtual void Die()
    {
        OnDie?.Invoke();
        Destroy(gameObject, 0.1f);
    }

    protected float TickTimer(float timer)
    {
        timer = Mathf.Max(timer - Time.deltaTime, 0);
        return timer;
    }

    protected virtual void HandleMovement()
    {
        if (ShouldTurn())
            direction = -direction;

        if (knockbackTimer > 0)
            return;

        Move();
    }

    protected virtual void Move()
    {

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
        foreach (Animator anim in eyeAnim)
            SetAnimValue(anim, parameter, value);
    }

    protected virtual void OnLanded()
    {

    }
   

    
    protected virtual bool ShouldTurn()
    {
        return ShouldTurnByObstacle() || ShouldTurnByPlayer();
    }

    protected virtual bool ShouldTurnByObstacle()
    {
        return false;
    }
    protected virtual bool ShouldTurnByPlayer()
    {
        return false;
    }

    protected virtual bool CanAttack()
    {
        return true;
    }

    public virtual void Attack(Player player)
    {
        if (CanAttack())
            OnAttack(player);
        else
            OnTouch(player);
    }
    protected virtual void OnAttack(Player player)
    {
        player.HitDamage(
                damage,
                knockbackPower,
                direction,
                knockbackTime
                );
    }
    protected virtual void OnTouch(Player player)
    {
        player.TouchHitDamage(damage);
    }

    protected virtual void PlayerFrontCheck()
    {
        Vector2 forward = direction > 0 ? Vector2.right : Vector2.left;

        isPlayerAhead = CheckPlayer(forward);
    }
    protected virtual void PlayerBackCheck()
    {
        Vector2 back = direction > 0 ? Vector2.left : Vector2.right;

        isPlayerBehind= CheckPlayer(back);
    }
    protected bool CheckPlayer(Vector2 dir)
    {
        return Physics2D.Raycast(
            playerRay.position,
            dir,
            playerRayLength,
            playerLayer
        );
    }

    public virtual void HitDamage(int damage, Vector2 knockback, float direction, float knockbackTime)
    {
        if (invincibleTimer > 0) return;
        health -= damage;
        rb.linearVelocity = new Vector2(knockback.x * direction, knockback.y);
        knockbackTimer = knockbackTime;
        invincibleTimer = invincibleTime;
        audioSource.PlayOneShot(hitSE);
        if (health <= 0) Die();
    }

    public void Initialize(float direction)
    {
        this.direction = direction;
    }
}