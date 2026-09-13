using System.Threading;
using UnityEngine;

public class SlimeEnemy : GroundEnemy
{
    [Header("Charge")]
    [SerializeField] protected float chargeSpeed;

    [SerializeField] protected float chargeTime;
    protected float chargeTimer;

    protected bool isCharging;

    public bool IsCharging => isCharging;

    protected override void Update()
    {
        base.Update();
        HandleAI();
    }
    protected override void HandleTimer()
    {
        base.HandleTimer();
        chargeTimer = TickTimer(chargeTimer);
    }

    protected override void HandleAnimation()
    {
        isFocusing = isCharging;
        SetAnim("IsCharging", isCharging);
        base.HandleAnimation();
    }

    protected override float GetMoveSpeed()
    {
        if (isCharging)
            return chargeSpeed;
        return base.GetMoveSpeed();
    }
    protected override bool ShouldTurn()
    {
        if (isCharging)
            return false;

        return base.ShouldTurn();
    }


    protected override void HandleAI()
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
            OnChargeFinished();
        }
        wasPlayerAhead = isPlayerAhead;
    }

    

    
    protected virtual void OnChargeFinished()
    {
        isCharging = false;
    }

   
}