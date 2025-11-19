using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public Transform player;
    public Vector2 minBounds = new Vector2(-8, -5);
    public Vector2 maxBounds = new Vector2(8, 5);

    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    void LateUpdate()
    {
        Vector3 targetPos = player.position;

        float clampedX = Mathf.Clamp(targetPos.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(targetPos.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}

