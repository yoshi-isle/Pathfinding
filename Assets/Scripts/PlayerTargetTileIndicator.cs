using UnityEngine;

public class PlayerTargetTileIndicator : MonoBehaviour
{
    PlayerController pc;
    Vector2 target;

    void Start()
    {
        pc = FindFirstObjectByType<PlayerController>();
        WorldClickRequest.WorldClickEvent += WorldClickEvent;
    }

    private void WorldClickEvent(WorldClickRequest request)
    {
        target = new Vector2(request.X, request.Y);
    }

    private void Update()
    {
        transform.position = new(target.x, 0.03f, target.y);
        if (transform.position.x == pc.transform.position.x && transform.position.z == pc.transform.position.z)
        {
            //TODO - On/Off
            transform.position = Vector3.zero;
        }
    }
}
