using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory : MonoBehaviour
{
    public int Capacity { get; set; }
    public Dictionary<int, Item?> Backpack { get; set; }
    public Dictionary<int, Item?> Equipment { get; set; }

    public Inventory()
    {
        Backpack = new Dictionary<int, Item?>();
        Equipment = new Dictionary<int, Item?>();
        for (int i = 0; i < Capacity; i++)
        {
            Backpack.Add(i, null);
        }

        for (int i = 0; i < 7; i++)
        {
            Equipment.Add(i, null);
        }
    }
}