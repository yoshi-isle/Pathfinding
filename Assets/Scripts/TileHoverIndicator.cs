using Unity.VisualScripting;
using UnityEngine;

public class TileHoverIndicator : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    bool Toggle;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            Vector2 gridLocation = WorldGrid.instance.WorldLocationToGrid(hit.point);
            if (WorldGrid.instance.InBounds(gridLocation))
            {
                Toggle = true;
                transform.position = new Vector3(Mathf.Round(hit.point.x), 0.1f, Mathf.Round(hit.point.z));
            }
            else
            {
                Toggle = false;
            }
        }
    }

    void LateUpdate()
    {
        meshRenderer.enabled = Toggle;
    }
}