using UnityEngine;
using System.Collections.Generic;

public class AStarPathfinder : MonoBehaviour
{
    public Vector2 target;
    private List<Vector2> path;
    public static AStarPathfinder instance;

    public void Start()
    {
        instance = this;
    }

    public List<Vector2> FindPath(Vector2 start, Vector2 goal)
    {
        start = new Vector2(Mathf.Ceil(start.x), Mathf.Ceil(start.y));
        goal = new Vector2(Mathf.Ceil(goal.x), Mathf.Ceil(goal.y));
        List<Vector2> openSet = new List<Vector2> { start };
        HashSet<Vector2> closedSet = new HashSet<Vector2>();
        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();
        Dictionary<Vector2, float> gScore = new Dictionary<Vector2, float> { { start, 0 } };
        Dictionary<Vector2, float> fScore = new Dictionary<Vector2, float> { { start, Heuristic(start, goal) } };

        while (openSet.Count > 0)
        {
            Vector2 current = GetLowestFScore(openSet, fScore);
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            // Add unwalkable world grid tiles to the closed set
            for (int x = 0; x < WorldGrid.instance.gridSize.x; x++)
            {
                for (int y = 0; y < WorldGrid.instance.gridSize.y; y++)
                {
                    if (!WorldGrid.instance.Walkable(new Vector2(x, y)))
                    {
                        closedSet.Add(new Vector2(x, y));
                    }
                }
            }

            foreach (Vector2 neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                if (neighbor.x < 0 || neighbor.x >= WorldGrid.instance.gridSize.x || neighbor.y < 0 || neighbor.y >= WorldGrid.instance.gridSize.y)
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + Vector2.Distance(current, neighbor);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);
            }
        }

        return new List<Vector2>();
    }

    private float Heuristic(Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b);
    }

    private Vector2 GetLowestFScore(List<Vector2> openSet, Dictionary<Vector2, float> fScore)
    {
        Vector2 lowest = openSet[0];
        foreach (Vector2 node in openSet)
        {
            if (fScore[node] < fScore[lowest])
            {
                lowest = node;
            }
        }
        return lowest;
    }

    private List<Vector2> GetNeighbors(Vector2 node)
    {
        List<Vector2> neighbors = new()
        {
            new(node.x + 1, node.y),
            new(node.x - 1, node.y),
            new(node.x, node.y + 1),
            new(node.x, node.y - 1),
            new(node.x + 1, node.y + 1),
            new(node.x - 1, node.y - 1),
            new(node.x + 1, node.y - 1),
            new(node.x - 1, node.y + 1)
        };
        return neighbors;
    }

    private List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
    {
        List<Vector2> totalPath = new List<Vector2> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }

    private void OnDrawGizmos()
    {
        if (path != null && path.Count > 0)
        {
            Gizmos.color = Color.red;
            foreach (Vector2 point in path)
            {
                Gizmos.DrawCube(new Vector3(point.x, 0, point.y), Vector3.one * 0.2f);
            }
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector3(target.x, 0, target.y), new Vector3(0.2f, 3f, 0.2f));
    }
}