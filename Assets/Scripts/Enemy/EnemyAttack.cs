using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Enemy enemy;
    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("PlayerHit"))
            return;

        Player player = other.GetComponentInParent<Player>();

        if (player == null) return;

        enemy.Attack(player);
    }
}
