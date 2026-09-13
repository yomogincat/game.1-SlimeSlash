using UnityEngine;

public class AlertSlime : SlimeEnemy
{
    [SerializeField] protected float counterChargeTime = 1.0f;
    protected override void HandlePlayerCheck()
    {
        base.HandlePlayerCheck();

        PlayerBackCheck();
    }

    protected override bool ShouldTurnByPlayer()
    {
        return isPlayerBehind;
    }

    public override void HitDamage(int damage, Vector2 knockback, float direction, float knockbackTime)
    {
        isCharging = true;
        chargeTimer = counterChargeTime;
        base.HitDamage(damage, knockback, direction, knockbackTime);
    }
}
