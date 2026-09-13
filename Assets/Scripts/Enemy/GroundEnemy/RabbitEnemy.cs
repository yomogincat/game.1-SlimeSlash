using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class RabbitEnemy : GroundEnemy
{
    [Header("Jump")]
    [SerializeField] float jumpPower = 20f;
    [SerializeField] float jumpInterval = 2f;
    [SerializeField] float jumpChargeTime = 0.2f;
    [SerializeField] float jumpMoveSpeed = 6f;

    float jumpTimer;
    float jumpChargeTimer;

    bool isJumping;
    bool isJumpCharging;


    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        HandleJump();
    }

    protected override void HandleTimer()
    {
        base.HandleTimer();

        jumpTimer = TickTimer(jumpTimer);

        jumpChargeTimer = TickTimer(jumpChargeTimer);
    }
    protected override void HandleAnimation()
    {
        isFocusing = isJumpCharging;
        SetAnim("IsJumping", isJumping);
        SetAnim("IsJumpCharging", isJumpCharging);
        base.HandleAnimation();
    }

    protected override void OnLanded()
    {
        if (isJumping)
        {
            jumpTimer = jumpInterval;
            isJumping = false;
        }
    }

    protected override float GetMoveSpeed()
    {
        if (isJumping)
            return jumpMoveSpeed;
        else if (isJumpCharging)
            return 0f;
        return base.GetMoveSpeed();
    }

    protected override bool ShouldTurn()
    {
        if (isJumpCharging)
            return false;
        return base.ShouldTurn();
    }

    protected override bool CanAttack()
    {
        return isJumping;
    }

    protected override void OnTouch(Player player)
    {
    }
    protected void HandleJump()
    {       
        if (jumpChargeTimer <= 0 && isJumpCharging)
        {
            isJumpCharging = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isJumping = true;
        }
            
    }

    protected override void HandleAI()
    {
        if (jumpTimer <= 0)
        {
            if (knockbackTimer > 0 || !isGrounded)
            {
                jumpTimer = jumpInterval;
                isJumpCharging = false;
                return;
            }
            if (!isJumpCharging)
            {
                isJumpCharging = true;
                jumpChargeTimer = jumpChargeTime;
            }
        }
    }

}