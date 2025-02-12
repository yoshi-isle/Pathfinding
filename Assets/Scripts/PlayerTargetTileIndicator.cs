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
        transform.position = new(pc.targetTile.x, 0.03f, pc.targetTile.y);
        if (transform.position.x == pc.transform.position.x && transform.position.z == pc.transform.position.z)
        {
            //temp
            transform.position = Vector3.zero;
        }
    }
}
