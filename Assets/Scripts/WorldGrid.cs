using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(300, 300); // Use Vector2Int for integer grid
    public int chunkSize = 16; // Size of each chunk
    public static WorldGrid instance;
    // Chunks: Dictionary<chunkPos, Dictionary<tilePos, TileInfo>>
    private Dictionary<Vector2Int, Dictionary<Vector2Int, TileInfo>> chunks = new();
    public int gizmoDrawRadius = 10; // Only draw tiles near player

    void Awake()
    {
        instance = this;
    }

    // Lazy tile access
    public TileInfo GetTile(Vector2Int gridPos)
    {
        Vector2Int chunkPos = new Vector2Int(gridPos.x / chunkSize, gridPos.y / chunkSize);
        Vector2Int localPos = new Vector2Int(gridPos.x % chunkSize, gridPos.y % chunkSize);
        if (localPos.x < 0) localPos.x += chunkSize;
        if (localPos.y < 0) localPos.y += chunkSize;
        if (!chunks.TryGetValue(chunkPos, out var chunk))
        {
            chunk = new Dictionary<Vector2Int, TileInfo>();
            chunks[chunkPos] = chunk;
        }
        if (!chunk.TryGetValue(localPos, out var tile))
        {
            tile = new TileInfo { Walkable = Walkable(new Vector2(gridPos.x, gridPos.y)) };
            chunk[localPos] = tile;
        }
        return tile;
    }

    public bool InBoundsAndWalkable(Vector2 worldLocation)
    {
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(worldLocation.x), Mathf.RoundToInt(worldLocation.y));
        if (gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= gridSize.x || gridPos.y >= gridSize.y)
            return false;
        return GetTile(gridPos).Walkable;
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

    public Vector2Int WorldLocationToGrid(Vector3 worldLocation)
    {
        return new Vector2Int(Mathf.RoundToInt(worldLocation.x), Mathf.RoundToInt(worldLocation.z));
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying && instance != null)
        {
            var player = GameObject.FindWithTag("Player");
            if (player == null) return;
            Vector2Int playerGrid = WorldLocationToGrid(player.transform.position);
            for (int x = -gizmoDrawRadius; x <= gizmoDrawRadius; x++)
            {
                for (int y = -gizmoDrawRadius; y <= gizmoDrawRadius; y++)
                {
                    Vector2Int pos = playerGrid + new Vector2Int(x, y);
                    if (pos.x < 0 || pos.y < 0 || pos.x >= gridSize.x || pos.y >= gridSize.y) continue;
                    var tile = GetTile(pos);
                    Gizmos.color = tile.Walkable ? Color.white : Color.red;
                    Gizmos.DrawCube(new Vector3(pos.x, 0, pos.y), Vector3.one * 0.5f);
                }
            }
        }
    }

    public Dictionary<Vector2, Vector2> GetRunCollisionTravelRules => new() {
        { new Vector2(-2, 2), new Vector2(-1, 1) },
        { new Vector2(-2, 1), new Vector2(-1, 1) },
        { new Vector2(-2, 0), new Vector2(-1, 0) },
        { new Vector2(-2, -1), new Vector2(-1, -1) },
        { new Vector2(-2, -2), new Vector2(-1, -1) },
        { new Vector2(-1, 2), new Vector2(-1, 1) },
        { new Vector2(0, 2), new Vector2(0, 1) },
        { new Vector2(1, 2), new Vector2(1, 1) },
        { new Vector2(2, 2), new Vector2(1, 1) },
        { new Vector2(2, 1), new Vector2(1, 1) },
        { new Vector2(2, 0), new Vector2(1, 0) },
        { new Vector2(2, -1), new Vector2(1, -1) },
        { new Vector2(2, -2), new Vector2(1, -1) },
        { new Vector2(1, -2), new Vector2(1, -1) },
        { new Vector2(0, -2), new Vector2(0, -1) },
        { new Vector2(-1, -2), new Vector2(-1, -1) }
    };
}
