using UnityEngine;

public class CameraData : MonoBehaviour
{
    private static CameraData instance;
    public static CameraData Instance => instance;

    public Vector3 CameraAngle { get; private set; }
    public Vector3 CameraPosition { get; private set; }
    public int CameraZoomAmount { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateCameraData(Vector3 angle, Vector3 position, int zoom)
    {
        CameraAngle = angle;
        CameraPosition = position;
        CameraZoomAmount = zoom;
    }
}
