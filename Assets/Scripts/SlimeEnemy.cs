using System.Threading;
using UnityEngine;

public class SlimeEnemy : Enemy
{
    [Header("Charge")]
    [SerializeField] float chargeSpeed;

    [SerializeField] float chargeTime;
    float chargeTimer;
    
    bool isCharging;

    public bool IsCharging => isCharging;

    protected override void Update()
    {
        base.Update();
        HandleAttack();
    }
    protected override void HandleTimer()
    {
        base.HandleTimer();
        chargeTimer = TickTimer(chargeTimer);
    }

    protected override void HandleAnimation()
    {
        base.HandleAnimation();
        SetAnim("IsCharging", isCharging);
    }
    protected override void HandleMovement()
    {
        base.HandleMovement();
        if (isKnockback) return;
        if (isCharging)
        {
            moveSpeed = chargeSpeed * direction;
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            return;
        }
    }
    void HandleAttack()
    {
        if (isPlayerAhead)
        {
            isCharging = true;
        }
        if (wasPlayerAhead && !isPlayerAhead)
        {
            chargeTimer = chargeTime;
        }
        
        if (isCharging && chargeTimer <= 0 && !isPlayerAhead)
        {
            isCharging = false;
        }
        wasPlayerAhead = isPlayerAhead;
    }
   
}