using Mono.Cecil;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Slash : MonoBehaviour
{
    [Header("Transforms")]
    Transform attackPoint;
    Transform player;
    Vector3 defaultScale;
    Vector3 offset;
    [Header("Life")]
    [SerializeField] float lifeTime = 0.1f;
    [Header("Stats")]
    [SerializeField] int damage = 1;
    [SerializeField] Vector2 knockbackPower = new Vector2(10, 2);
    [SerializeField] float knockbackTime = 0.1f;

    float direction;
    public void Initialize(Transform attackPoint, Transform player, float direction)
    {
        this.player = player;
        offset = attackPoint.localPosition;
        this.direction = direction;
        offset.x *= direction;
    }

    void Awake()
    {
        defaultScale = transform.localScale;
    }
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("EnemyHit"))
            return;

        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy == null) return;
        {
            enemy.HitDamage(damage,
                knockbackPower,
                direction,
                knockbackTime
                );
        }
    }
    private void LateUpdate()
    {
        transform.localScale = new Vector3(direction, defaultScale.y, defaultScale.x);

        transform.position = new Vector3(
            player.position.x + offset.x,
            player.position.y + offset.y,
            transform.position.z
        );
    }
}
