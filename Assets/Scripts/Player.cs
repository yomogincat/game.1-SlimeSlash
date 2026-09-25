using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //==================== References ====================

    [Header("References")]
    Rigidbody2D rb;

    [SerializeField] Animator graphicsAnim;
    [SerializeField] Animator eyeAnim;
    [SerializeField] AudioSource audioSource;

    [SerializeField] SpriteRenderer bodyRenderer;
    [SerializeField] SpriteRenderer eyeRenderer;

    [SerializeField] HPUI hpUI;
    public Slash slashPrefab;

    [SerializeField] AbilityManager abilityManager;

    //==================== Transforms ====================

    [Header("Transforms")]
    [SerializeField] Transform graphics;
    [SerializeField] Transform attackPoint;

    [SerializeField] Transform groundRayR;
    [SerializeField] Transform groundRayL;
    [SerializeField] Transform wallRayR;
    [SerializeField] Transform wallRayL;

    //==================== Layers ====================

    [Header("Layers")]
    [SerializeField] LayerMask groundLayer;

    //==================== Sounds ====================

    [Header("Sounds")]
    [SerializeField] AudioClip slashWhoosh;
    [SerializeField] AudioClip hit_impact;
    //==================== Stats ====================

    [Header("Stats")]
    [SerializeField] int maxHealth = 5;
    int health;

    [SerializeField] Vector2 touchKnockback = new Vector2(6,2);
    [SerializeField] float touchKnockbackTime;
    //==================== Energy ====================

    [Header("Energy")]
    [SerializeField] float maxEnergy = 100f;
    [SerializeField] float energyRegen = 5f;

    [SerializeField] float energy;


    //==================== Movement ====================

    [Header("Movement")]
    [SerializeField] float walkSpeed = 8f;

    [SerializeField] float jumpPower = 20f;
    [SerializeField] float jumpCutScale = 0.5f;
    [SerializeField] int maxJump = 1;
    int jumpCount;


    [SerializeField] float wallJumpPower = 20f;
    float wallJumpDirection;
    float wallDirection;

    float wallJumpBoost;


    float direction = 1f;
    float facingDirection = 1f;

    float moveSpeed;

    float dashDirection;

    //==================== Gravity ====================

    [Header("Gravity")]
    [SerializeField] float normalGravity = 5f;
    [SerializeField] float fallGravity = 8f;
    [SerializeField] float jumpCutGravity = 15f;
    [SerializeField] float wallSlideGravity = 3f;

    [SerializeField] float maxFallSpeed = -20f;
    [SerializeField] float wallSlideSpeed = -5f;

    //==================== Attack ====================

    [Header("Attack")]
    [SerializeField] float slashCooldownMax;
    float slashCooldown;

    //==================== Abilities ===================
    [Header("Abilities")]

    DashParameters currentDash;

    //==================== Costs ====================

    [Header("Costs")]

    //==================== Colors ====================

    [Header("Colors")]
    [SerializeField] float eyeFlashAlpha;
    [SerializeField] float bodyFlashAlpha;
    bool isFlashing;

    //==================== Timers ====================

    [Header("Timers")]
    [SerializeField] float invincibleTime = 0.2f;
    [SerializeField] float dashBufferTime = 0.1f;
    [SerializeField] float dashCoolTime = 0.6f;
    [SerializeField] float jumpBufferTime = 0.1f;
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float wallJumpTime = 0.25f;
    [SerializeField] float jumpCutBufferTime = 0.1f;
    [SerializeField] float jumpCutEffectTime = 0.2f;
    [SerializeField] float flashTime = 0.06f;

    float invincibleTimer;
    float dashTimer;
    float dashBufferTimer;
    float dashCoolTimer;
    float jumpBufferTimer;
    float coyoteTimer;
    float jumpCutBufferTimer;
    float knockbackTimer;
    float wallJumpTimer;
    float flashTimer;

    //==================== Checks ====================

    [Header("Checks")]
    [SerializeField] float groundRayLength = 0.2f;
    [SerializeField] float wallRayLength = 0.2f;

    RaycastHit2D groundHitR;
    RaycastHit2D groundHitL;
    RaycastHit2D wallHitR;
    RaycastHit2D wallHitL;

    bool isGroundedR;
    bool isGroundedL;

    bool isTouchingWallR;
    bool isTouchingWallL;

    bool isTouchingFrontWall;
    bool isTouchingBackWall;

    //==================== Runtime ====================
    bool canDash;
    //==================== Properties ====================
    public int Health => health;
    public int MaxHealth => maxHealth;
    public float Energy => energy;
    public float MaxEnergy => maxEnergy;

    public float Direction => direction;
    //==================== State ====================

    bool isGrounded;
    bool isTouchingWall;
    bool isWallSliding;
    bool isJumping;
    bool isWallJumping;
    bool isDashing;
    bool isKnockback;

    //==================== Input ====================

    float moveInput;
    float verticalInput;
    bool shootInput;
    bool jumpPressed;
    bool jumpReleased;
    bool attackInput;

    int abilitySlotInput;

    //==================== Utility ====================

    bool resetVY;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Respawn();
    }

    void Update()
    {
        HandleInput();

        HandleInputBuffer();

        HandleRespawn();

        HandleEnergy();

        HandleTimers();

        HandleAttack();

        HandleAbilities();

        HandleDirection();


        HandleSpriteDirection();

        HandleInvincibleEffect();

        HandleAnimation();

        HandleDebug();
  
    }
    void FixedUpdate()
    {
        HandleGroundCheck();

        HandleWallCheck();

        HandleWallSliding();

        HandleDash();

        HandleJump();

        HandleMovement();

        HandleGravity();

        HandleVelocityControl();
    }

    void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        jumpPressed = Input.GetButtonDown("Jump");
        jumpReleased = Input.GetButtonUp("Jump");

        attackInput = Input.GetButtonDown("Attack1");

        abilitySlotInput =
            Input.GetKeyDown(KeyCode.Q) ? 0 :
            Input.GetKeyDown(KeyCode.W) ? 1 :
            Input.GetKeyDown(KeyCode.E) ? 2 :
            -1;

    }

    void HandleInputBuffer()
    {
        if (jumpPressed) jumpBufferTimer = jumpBufferTime;
        if (jumpReleased && rb.linearVelocity.y > 0)
            jumpCutBufferTimer = jumpCutBufferTime;
    }


    void HandleTimers()
    {
        dashCoolTimer = TickTimer(dashCoolTimer);
        jumpBufferTimer = TickTimer(jumpBufferTimer);
        if (!isGrounded) coyoteTimer = TickTimer(coyoteTimer);
        slashCooldown = TickTimer(slashCooldown);
        wallJumpTimer = TickTimer(wallJumpTimer);
        jumpCutBufferTimer = TickTimer(jumpCutBufferTimer);
        dashBufferTimer = TickTimer(dashBufferTimer);
        knockbackTimer = TickTimer(knockbackTimer);
        invincibleTimer = TickTimer(invincibleTimer);
        flashTimer = TickTimer(flashTimer);
        dashTimer = TickTimer(dashTimer);
    }

    void HandleDebug()
    {
        Debug.DrawRay(
            groundRayR.position,
            Vector2.down * groundRayLength,
            isGroundedR ? Color.green : Color.red
            );

        Debug.DrawRay(
            groundRayL.position,
            Vector2.down * groundRayLength,
            isGroundedL ? Color.green : Color.red
            );

        Debug.DrawRay(
            wallRayR.position,
            Vector2.right * wallRayLength,
            isTouchingWallR ? Color.green : Color.red
            );

        Debug.DrawRay(
            wallRayL.position,
            Vector2.left * wallRayLength,
            isTouchingWallL ? Color.green : Color.red
            );
    }
    void HandleRespawn()
    {
        if (transform.position.y < -10) Respawn();
        if (health <= 0) Respawn();
    }
    void HandleVelocityControl()
    {
        if (resetVY)
        {
            rb.linearVelocityY = 0;
            resetVY = false;
        }
        if (isWallSliding) rb.linearVelocityY = Mathf.Max(rb.linearVelocity.y, wallSlideSpeed);
        else rb.linearVelocityY = Mathf.Max(rb.linearVelocity.y, maxFallSpeed);
    }

    void HandleGravity()
    {
        if (isWallSliding)
        {
            rb.gravityScale = wallSlideGravity;
            return;
        }
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = fallGravity;
            return;
        }
        else
        {
            rb.gravityScale = normalGravity;
            return;
        }
    }
    void HandleGroundCheck()
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
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            jumpCount = 0;
        }
    }

        void HandleWallCheck()
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

        wallDirection =
            isTouchingWallR ? 1 :
            isTouchingWallL ? -1 :
            0;
    }
    void HandleEnergy()
    {
        if (energy < maxEnergy)
        {
            energy = Mathf.Min(energy + energyRegen * Time.deltaTime, maxEnergy);
        }
    }
    void HandleAnimation()
    {
        SetAnim("Speed", Mathf.Abs(moveInput));
        SetAnimValue(graphicsAnim, "YVelocity", rb.linearVelocity.y);
        SetAnimValue(graphicsAnim, "IsGrounded", isGrounded);
        SetAnimValue(graphicsAnim, "IsWallSliding", isWallSliding); 
    }

    void HandleInvincibleEffect()
    {
        Color eyeColor = eyeRenderer.color;
        Color bodyColor = bodyRenderer.color;
        if (invincibleTimer <= 0)
        {
            eyeColor = Color.white;
            bodyColor = Color.white;
        }
        else if (flashTimer <= 0)
        {
            if (!isFlashing)
            {
                eyeColor.a = eyeFlashAlpha;
                bodyColor.a = bodyFlashAlpha;
                isFlashing = true;
            }
            else
            {
                eyeColor = Color.white;
                bodyColor = Color.white;
                isFlashing = false;
            }
            flashTimer = flashTime;
        }
        eyeRenderer.color = eyeColor;
        bodyRenderer.color = bodyColor;
    }
        

    void HandleSpriteDirection()
    {
        facingDirection = direction;

        if (isWallSliding)
        {
            facingDirection = -wallDirection;
        }

        graphics.localScale = new Vector3(facingDirection, 1, 1);
    }
    void HandleDirection()
    {
        if (moveInput != 0) direction = Mathf.Sign(moveInput);
    }

    void HandleWallSliding()
    {
        isWallSliding =
            isTouchingFrontWall
            && !isGrounded
            && !isWallJumping
            && moveInput != -wallDirection;
    }


    //======
    // Move
    //======
    void HandleMovement()
    {
        if (isKnockback)
        {
            isKnockback = knockbackTimer > 0;
            return;
        }
        

        if (isWallJumping)
        {
            moveSpeed = wallJumpBoost + moveInput * walkSpeed;
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            isWallJumping = wallJumpTimer > 0;
            return;
        }

        if (dashTimer > 0)
        {
            moveSpeed = dashDirection * currentDash.Speed + moveInput * currentDash.Control;
            rb.linearVelocity = new Vector2(moveSpeed, 0);
            return;
        }

        moveSpeed = moveInput * walkSpeed;
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }

    public void HandleDash()
    {
        if (isGrounded || isTouchingWall && dashCoolTimer <= 0) canDash = true;

        if (isDashing && dashTimer <= 0)
        {
            dashCoolTimer = dashCoolTime;
            isDashing = false;
        }
    }
    public void Dash(DashParameters config)
    {
        currentDash = config;
        if (canDash && energy >= currentDash.Cost)
        {
            energy -= currentDash.Cost;
            dashTimer = currentDash.Time;
            dashDirection = facingDirection;
            dashBufferTimer = 0;
            resetVY = true;
            canDash = false;
            isDashing = true;
        }
    }
    void HandleJump()
    {
        if (jumpBufferTimer > 0)
        {
            if (CanWallJump() && !isWallJumping)
            {
                wallJumpDirection = wallDirection;
                wallJumpBoost = wallJumpPower * -wallJumpDirection;
                wallJumpTimer = wallJumpTime;
                isWallJumping = true;
                jumpBufferTimer = 0;
                isJumping = true;
                Jump();
                jumpCutBufferTimer = 0;
            }
            else if (CanJump())
            {
                jumpBufferTimer = 0;
                isJumping = true;
                Jump();
            }
        }
        if (isJumping)
            isJumping = !(isWallSliding || isGrounded) || rb.linearVelocity.y > 0;
        if (jumpCutBufferTimer > 0 && rb.linearVelocity.y > 0)
        {
            rb.linearVelocityY = rb.linearVelocity.y * jumpCutScale;
            Debug.Log($"JumpCut frame:{Time.frameCount}");
            jumpCutBufferTimer = 0;
        }
    }

    void HandleAttack()
    {
        if (attackInput && slashCooldown <= 0 && !isKnockback)
        {
            Slash slash = Instantiate(slashPrefab, attackPoint.position, Quaternion.identity);
            slash.Initialize(attackPoint, transform, facingDirection);
            slashCooldown = slashCooldownMax;
            audioSource.PlayOneShot(slashWhoosh);
        }
    }

    void HandleAbilities()
    {
        if (abilitySlotInput == -1) return;
        abilityManager.UseAbility(abilitySlotInput);
    }

    //=========
    // Utility
    //=========
    void Respawn()
    {
        transform.position = new Vector3(0, 0, 0);
        resetVY = true;
        energy = maxEnergy;
        health = maxHealth;
        invincibleTimer = invincibleTime;
        hpUI.GenerateOrbs(maxHealth);
    }
    bool CanJump()
    {
            return ((isGrounded || coyoteTimer > 0)
                && jumpCount < maxJump);
    }

    bool CanWallJump()
    {
        return isTouchingFrontWall && !isGrounded;
    }
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        jumpCount++;
    }
    IEnumerator HitStop(float time)
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(time);

        Time.timeScale = 1f;
    }
    float TickTimer(float timer)
    {
        timer = Mathf.Max(timer - Time.deltaTime, 0);
        return timer;
    }

    void SetAnimValue<T>(Animator anim, string parameter,T value)
    {
        if (anim == null) return;
        if (value is int i)
            anim.SetInteger(parameter, i);
        else if (value is float f)
            anim.SetFloat(parameter, f);
        else if (value is bool b)
            anim.SetBool(parameter, b);
    }
    
    void SetAnim<T>(string parameter, T value)
    {
        SetAnimValue(graphicsAnim, parameter, value);
        SetAnimValue(eyeAnim, parameter, value);
    }

    public void TouchHitDamage(int damage)
    {
        if (invincibleTimer > 0) return;
        health -= damage;
        isKnockback = true;
        knockbackTimer = touchKnockbackTime;
        rb.linearVelocity = new Vector2(touchKnockback.x * -direction, touchKnockback.y);
        invincibleTimer = invincibleTime;
        audioSource.PlayOneShot(hit_impact);
        StartCoroutine(HitStop(0.08f));
    }
    public void HitDamage(int damage, Vector2 knockback, float direction, float knockbackTime)
    {
        if (invincibleTimer > 0) return;
        health -= damage;
        isKnockback = true;
        knockbackTimer = knockbackTime;
        invincibleTimer = invincibleTime;
        rb.linearVelocity = new Vector2(knockback.x * direction, knockback.y);
        audioSource.PlayOneShot(hit_impact);
        StartCoroutine(HitStop(0.1f));
    }
    // void OnDrawGizmos()
} 