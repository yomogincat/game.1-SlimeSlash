using UnityEngine;

public class CameraControl : MonoBehaviour
{
	private Player player;

	private RoomManager roomManager;

	private Camera camera;

	[SerializeField]
	private float minX;

	[SerializeField]
	private float maxX;

	[SerializeField]
	private float minY;

	[SerializeField]
	private float maxY;

	[SerializeField]
	private float followSpeed;

	[SerializeField]
	private float lookAheadDistance = 1f;

	[SerializeField]
	private float lookAheadSpeed;

	private float currentLookAhead;

	[SerializeField]
	private float deadZoneX;

	[SerializeField]
	private float deadZoneY;

	private void Start()
	{
		player = Object.FindFirstObjectByType<Player>();
		roomManager = Object.FindFirstObjectByType<RoomManager>();
		camera = GetComponent<Camera>();
	}

	private void LateUpdate()
	{
		float orthographicSize = camera.orthographicSize;
		float num = orthographicSize * camera.aspect;
		Bounds bounds = roomManager.CurrentRoom.CameraBounds.bounds;
		float b = player.Direction * lookAheadDistance;
		currentLookAhead = Mathf.Lerp(currentLookAhead, b, lookAheadSpeed);
		Vector3 position = player.transform.position;
		position.x += currentLookAhead;
		Vector3 position2 = Vector3.Lerp(base.transform.position, position, followSpeed);
		position2.x = Mathf.Clamp(position2.x - position.x, 0f - deadZoneX, deadZoneX) + position.x;
		position2.y = Mathf.Clamp(position2.y - position.y, 0f - deadZoneY, deadZoneY) + position.y;
		if (roomManager.CurrentRoom != null)
		{
			position2.x = Mathf.Clamp(position2.x, bounds.min.x + num, bounds.max.x - num);
			position2.y = Mathf.Clamp(position2.y, bounds.min.y + orthographicSize, bounds.max.y - orthographicSize);
			position2.z = base.transform.position.z;
		}
		base.transform.position = position2;
	}
}
