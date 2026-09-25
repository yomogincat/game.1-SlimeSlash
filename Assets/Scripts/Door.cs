using System;
using UnityEngine;

public class Door : MonoBehaviour
{
	public enum DoorDirection
	{
		Right = 0,
		Left = 1,
		Up = 2,
		Down = 3
	}

	[SerializeField]
	private Transform spawnPoint;

	[SerializeField]
	private Door targetDoor;

	[SerializeField]
	private Room room;

	[SerializeField]
	private DoorDirection spawnDirection;

	public Transform SpawnPoint => spawnPoint;

	public Door TargetDoor => targetDoor;

	public Room Room => room;

	public event Action<Door> PlayerPassed;

	public event Action<Door> PlayerExited;

	private void Start()
	{
		GetDirection();
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			PlayerExited?.Invoke(this);
		}
		Vector2 lhs = other.GetComponentInParent<Player>().transform.position - base.transform.position;
		Vector2 direction = GetDirection();
		float num = Vector2.Dot(lhs, direction);
		Debug.Log($"Passed? side={num}");
		if (Mathf.Sign(num) < 0f)
		{
			PlayerPassed?.Invoke(this);
		}
	}

	private Vector2 GetDirection()
	{
		return spawnDirection switch
		{
			DoorDirection.Right => Vector2.right, 
			DoorDirection.Left => Vector2.left, 
			DoorDirection.Up => Vector2.up, 
			DoorDirection.Down => Vector2.down, 
			_ => Vector2.right, 
		};
	}

	private void OnDrawGizmos()
	{
		BoxCollider2D component = GetComponent<BoxCollider2D>();
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(base.transform.position, new Vector3(component.size.x, component.size.y, 1f));
		float num = 0.3f;
		Gizmos.DrawCube(spawnPoint.position, new Vector3(num, num, 1f));
	}
}
