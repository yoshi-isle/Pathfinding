using System;
using UnityEngine;

public class PlayerTrueTileIndicator : MonoBehaviour
{
    public Transform follow;

    void Start()
    {
        TickCounter.Instance.OnTick += OnTick;
    }

    private void OnTick()
    {
        transform.position = new(follow.position.x, 0.03f, follow.position.z);
    }
}