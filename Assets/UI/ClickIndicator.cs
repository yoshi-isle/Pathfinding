using System;
using UnityEngine;

public class ClickIndicator : MonoBehaviour
{
    public GameObject clickIndicatorObject;

    void Start()
    {
        PlayerController.WorldClick += OnWorldClick;
    }

    private void OnWorldClick(Vector3 mousePosition)
    {
        var clickIndicator = Instantiate(clickIndicatorObject, mousePosition, Quaternion.identity);
        clickIndicator.transform.SetParent(this.transform);
    }
}