using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Player player;


    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    [SerializeField] float followSpeed;

    [SerializeField] float lookAheadDistance = 1f;
    [SerializeField] float lookAheadSpeed;
    float currentLookAhead;

    [SerializeField] float deadZoneX;
    [SerializeField] float deadZoneY;
    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }
    void LateUpdate()
    {
        float targetLookAhead = player.Direction * lookAheadDistance;

        currentLookAhead = Mathf.Lerp(
            currentLookAhead,
            targetLookAhead,
            lookAheadSpeed
            );

        Vector3 targetPosition = player.transform.position;
        targetPosition.x += currentLookAhead;
        Vector3 pos = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed
            );

        pos.x = Mathf.Clamp(pos.x - targetPosition.x, -deadZoneX, deadZoneX) + targetPosition.x;
        pos.y = Mathf.Clamp(pos.y - targetPosition.y, -deadZoneY, deadZoneY) + targetPosition.y;


        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = transform.position.z;
        transform.position = pos;

    }

    
}
