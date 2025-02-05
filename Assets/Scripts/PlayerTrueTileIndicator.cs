using System;
using UnityEngine;

public class PlayerTrueTileIndicator : MonoBehaviour
{
    public Transform follow;

    void Start()
    {
        GameManager.Instance.OnTick += OnTick;
    }

    private void OnTick()
    {
        transform.position = new(follow.position.x, 0.1f, follow.position.z);
    }
}