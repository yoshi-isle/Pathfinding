using System;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

[Serializable]
public class Inventory
{
    public int Capacity;
    public List<Item> Backpack;

    public Inventory(int capacity)
    {
        Capacity = capacity;
        Backpack = new List<Item>();

        for (int i = 0; i < Capacity; i++)
        {
            Backpack.Add(null);
        }

        for (int i = 0; i < 2; i++)
        {
            Backpack.Add(new Item { Name = $"Test item {i}", Stackable = false });
        }
    }

    public Inventory(int capacity, List<Item> inventory)
    {
        Capacity = capacity;
        Backpack = inventory;
    }
}