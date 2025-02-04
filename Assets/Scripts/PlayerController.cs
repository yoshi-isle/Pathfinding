using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 targetTile;
    public UnitController uc;
    public List<Vector2> currentPath;
    public LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 0;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")) { color = Color.magenta };
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                targetTile = new Vector2(Mathf.Round(hit.point.x), Mathf.Round(hit.point.z));
                currentPath = uc.FindPath(new Vector2(transform.position.x, transform.position.z), targetTile);
            }
        }

        if (currentPath.Count != 0)
        {
            lineRenderer.positionCount = currentPath.Count;
            for (int i = 0; i < currentPath.Count; i++)
            {
                lineRenderer.SetPosition(i, new Vector3(currentPath[i].x, 0, currentPath[i].y));
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(new(targetTile.x, 0, targetTile.y), new(1, 1, 1));

        if (currentPath.Count != 0)
        {
            Gizmos.color = Color.magenta;

            foreach (var tile in currentPath)
            {
                Gizmos.DrawCube(new(tile.x, 0, tile.y), new(0.9f, 0.9f, 0.9f));
            }
        }
    }
}

