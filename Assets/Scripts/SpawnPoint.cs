using UnityEngine;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;




#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnPoint : MonoBehaviour
{

    [SerializeField] Enemy enemyPrefab;
    Player player;

    [SerializeField] float direction = -1;
    [SerializeField] float respawnTime;

    [SerializeField] Vector2 spawnRange;

    Enemy enemy;
    float respawnTimer;
    bool respawnStarted;


    void Awake()
    {
        player = FindFirstObjectByType<Player>();
    }

    void Start()
    {
        if (IsPlayerInRange())
            Spawn();
    }
            

    void Update()
    {
        bool isPlayerInRange = IsPlayerInRange();
        if (!isPlayerInRange)
            respawnTimer = Mathf.Max(respawnTimer - Time.deltaTime, 0);
        if (enemy == null && !respawnStarted)
        {
            respawnTimer = respawnTime;
            respawnStarted = true;
        }
        if (enemy == null && respawnTimer <= 0 && isPlayerInRange)
        {
            Spawn();
            respawnStarted = false;
        }
        
    }
    public void Spawn()
    {
        enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemy.Initialize(direction);
        enemy.OnDie += OnEnemyDie;
    }

    void OnEnemyDie()
    {
        enemy.OnDie -= OnEnemyDie;
        enemy = null;
    }

    bool IsPlayerInRange()
    {
        return Mathf.Abs(transform.position.x - player.transform.position.x) <= spawnRange.x
            && Mathf.Abs(transform.position.y - player.transform.position.y) <= spawnRange.y;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        float size = 0.3f;
        Gizmos.DrawCube(
            transform.position,
            new Vector3(size, size, 1f)
            );

    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Handles.Label(
            transform.position + Vector3.up,
            $"Enemy : {enemyPrefab.name}\n" +
            $"Respawn : {respawnTimer:F1}s"
        );
#endif
    }

}
