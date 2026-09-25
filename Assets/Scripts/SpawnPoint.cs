using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	[SerializeField]
	private Enemy enemyPrefab;

	private Player player;

	private RoomManager roomManager;

	private Enemy enemy;

	[SerializeField]
	private float direction = -1f;

	[SerializeField]
	private Room room;

	[SerializeField]
	private Vector2 spawnRange;

	[SerializeField]
	private bool respawn = true;

	private bool isPlayerInRange;

	private bool isPlayerInRoom;

	private bool hasDefeated;

	private bool hasExitedRoom;

	private bool hasSpawned;

	private void Awake()
	{
		if (room == null)
		{
			room = GetComponentInParent<Room>();
		}
		player = Object.FindFirstObjectByType<Player>();
		roomManager = Object.FindFirstObjectByType<RoomManager>();
	}

	private void Start()
	{
		if (IsPlayerInRange() && IsPlayerInRoom())
		{
			Spawn();
		}
	}

	private void Update()
	{
		if (!hasDefeated || respawn)
		{
			isPlayerInRoom = IsPlayerInRoom();
			isPlayerInRange = IsPlayerInRange();
			if (!isPlayerInRoom && !hasExitedRoom)
			{
				hasExitedRoom = true;
			}
			if (!IsPlayerInRoom() && enemy != null)
			{
				Object.Destroy(enemy.gameObject);
				enemy = null;
			}
			if (ShouldSpawn())
			{
				Spawn();
				hasExitedRoom = false;
			}
		}
	}

	private bool ShouldSpawn()
	{
		if (enemy == null && isPlayerInRange && isPlayerInRoom && (hasExitedRoom || !hasSpawned))
		{
			if (hasDefeated)
			{
				return respawn;
			}
			return true;
		}
		return false;
	}

	public void Spawn()
	{
		enemy = Object.Instantiate(enemyPrefab, base.transform.position, Quaternion.identity);
		enemy.Initialize(direction, room);
		enemy.OnDie += OnEnemyDie;
		if (!hasSpawned)
		{
			hasSpawned = true;
		}
	}

	private void OnEnemyDie()
	{
		enemy.OnDie -= OnEnemyDie;
		enemy = null;
		hasDefeated = true;
	}

	private bool IsPlayerInRange()
	{
		if (Mathf.Abs(base.transform.position.x - player.transform.position.x) <= spawnRange.x)
		{
			return Mathf.Abs(base.transform.position.y - player.transform.position.y) <= spawnRange.y;
		}
		return false;
	}

	private bool IsPlayerInRoom()
	{
		return roomManager.CurrentRoom == room;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		float num = 0.3f;
		Gizmos.DrawCube(base.transform.position, new Vector3(num, num, 1f));
	}

	private void OnDrawGizmosSelected()
	{
	}
}
