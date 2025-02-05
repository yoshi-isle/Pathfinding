using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 targetTile;
    public List<Vector2> currentPath;

    void Start()
    {
        targetTile = WorldGrid.instance.WorldLocationToGrid(transform.position);
        GameManager.Instance.OnTick += OnTick;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                Vector2 gridLocation = WorldGrid.instance.WorldLocationToGrid(hit.point);
                if (WorldGrid.instance.InBounds(gridLocation))
                {
                    targetTile = new Vector2(Mathf.Round(hit.point.x), Mathf.Round(hit.point.z));
                }
            }
        }
    }

    void OnTick()
    {
        Vector2 currentPos = new(transform.position.x, transform.position.z);

        // Only find new path if we're not at the target
        if (currentPos != targetTile)
        {
            currentPath = AStarPathfinder.instance.FindPath(currentPos, targetTile);

            if (currentPath != null && currentPath.Count > 1)
            {
                currentPath.RemoveAt(0);
                transform.position = new Vector3(currentPath.First().x, transform.position.y, currentPath.First().y);
            }
        }
        else
        {
            currentPath.Clear();
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

