using System;
using UnityEngine;

public class Room : MonoBehaviour
{
	[SerializeField]
	private BoxCollider2D room;

	private Bounds bounds;

	[SerializeField]
	private BoxCollider2D cameraBounds;

	public bool IsPlayerInside { get; private set; }

	public BoxCollider2D CameraBounds => cameraBounds;

	public Bounds Bounds => bounds;

	public event Action<Room> PlayerEntered;

	public event Action<Room> PlayerExited;

	private void Start()
	{
		bounds = room.bounds;
		if (cameraBounds == null)
		{
			cameraBounds = room;
		}
	}

	private void Update()
	{
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			IsPlayerInside = true;
			PlayerEntered?.Invoke(this);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			IsPlayerInside = false;
			PlayerExited?.Invoke(this);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube(base.transform.position, new Vector3(room.size.x, room.size.y, 1f));
	}
}
