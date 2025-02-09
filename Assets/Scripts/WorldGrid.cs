using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    public Vector2 gridSize;
    public static WorldGrid instance;
    public Dictionary<Vector2, TileInfo> Map { get; set; }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        BakeMap();
    }

    void BakeMap()
    {
        Map = new();

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                var tile = new TileInfo
                {
                    Walkable = Walkable(new(x, y))
                };

                Map.Add(new(x, y), tile);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (Map == null || Map.Count == 0) return;
        foreach (var item in Map)
        {
            Gizmos.color = item.Value.Walkable ? Color.white : Color.red;
            Gizmos.DrawCube(new(item.Key.x, 0, item.Key.y), Vector3.one * 0.5f);
        }
    }

    public bool Walkable(Vector2 worldLocation)
    {
        return !Physics.SphereCast(
            new Vector3(worldLocation.x, 10f, worldLocation.y),
            0.25f,
            Vector3.down,
            out RaycastHit hit,
            20f,
            LayerMask.GetMask("Blocker")
        );
    }


    public bool InBoundsAndWalkable(Vector2 worldLocation)
    {
        if (Map == null || Map.Count == 0 || !Map.ContainsKey(new(worldLocation.x, worldLocation.y))) return false;
        var hi = Map[new(worldLocation.x, worldLocation.y)].Walkable;
        return Map[new(worldLocation.x, worldLocation.y)].Walkable;
    }

    // TODO - Move
    public Vector2 WorldLocationToGrid(Vector3 worldLocation)
    {
        return new Vector2(Mathf.Round(worldLocation.x), Mathf.Round(worldLocation.z));
    }
}
