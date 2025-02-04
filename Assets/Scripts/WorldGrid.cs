using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    public Vector2 gridSize;
    public static WorldGrid instance;

    void Awake()
    {
        instance = this;
    }

    void OnDrawGizmos()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (Walkable(new(x, y)))
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawWireCube(new(x, 0, y), new(0.5f, 0.5f, 0.5f));
            }
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
}
