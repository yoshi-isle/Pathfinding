using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform follow;
    public Vector3 offset;
    Vector3 velocity;
    public float smoothTime = 0.2f;

    void Update()
    {
        Vector3 targetPosition = follow.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}