using System;
using System.Linq;
using UnityEngine;

public class PlayerTargetTileIndicator : MonoBehaviour
{
    public PlayerController pc;

    void Start()
    {
        pc = FindFirstObjectByType<PlayerController>();
    }

    private void Update()
    {
        if (pc.targetTile == null)
        {
            return;
        }

        Vector2 target = (Vector2)pc.targetTile;

        transform.position = new(target.x, 0.03f, target.y);
        if (transform.position.x == pc.transform.position.x && transform.position.z == pc.transform.position.z)
        {
            //temp
            transform.position = Vector3.zero;
        }
    }
}
