using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    SlimeEnemy enemy;
    //==================== Stats ====================
    [Header("Stats")]
    [SerializeField] int damage = 1;
    [SerializeField] Vector2 knockbackPower = new Vector2(5, 5);
    [SerializeField] float knockbackTime = 0.1f;
    void Awake()
    {
        enemy = GetComponentInParent<SlimeEnemy>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("PlayerHit"))
            return;

        Player player = other.GetComponentInParent<Player>();

        if (player == null) return;
        if (enemy.IsCharging)
        {
            player.HitDamage(
                damage,
                knockbackPower,
                enemy.Direction,
                knockbackTime
                );
        }
        else
        {
            player.TouchHitDamage(damage);
        }

        
    }
}
