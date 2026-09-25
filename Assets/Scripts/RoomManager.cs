using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
	[SerializeField]
	private List<Room> rooms = new List<Room>();

	private Room currentRoom;

	private Player player;

	public Room CurrentRoom => currentRoom;

	private void Start()
	{
		player = Object.FindFirstObjectByType<Player>();
		foreach (Room room in rooms)
		{
			room.PlayerEntered += OnPlayerEntered;
			room.PlayerExited += OnPlayerExited;
		}
		Door[] array = Object.FindObjectsByType<Door>(FindObjectsSortMode.None);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].PlayerPassed += OnDoorPassed;
		}
	}

	private void OnPlayerEntered(Room room)
	{
		currentRoom = room;
	}

	private void OnPlayerExited(Room room)
	{
	}

	private void OnDoorPassed(Door door)
	{
		Door targetDoor = door.TargetDoor;
		Debug.Log("Passed Door: " + door.name);
		Debug.Log("Target Door: " + targetDoor.name);
		Debug.Log("Target Room: " + targetDoor.Room.name);
		player.transform.position = targetDoor.SpawnPoint.position;
		currentRoom = targetDoor.Room;
		Debug.Log("Current after door: " + currentRoom.name);
	}
}
