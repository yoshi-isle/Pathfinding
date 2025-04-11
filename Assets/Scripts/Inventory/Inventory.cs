using System;
using System.Collections.Generic;

#nullable enable

[Serializable]
public class Inventory
{
    public int Capacity;
    public List<InventorySlot> Backpack;

    public Inventory(int capacity)
    {
        Capacity = capacity;
        Backpack = new List<InventorySlot>();

        for (int i = 0; i < Capacity; i++)
        {
            Backpack.Add(new InventorySlot(i));
        }
    }

    public Inventory(int capacity, List<InventorySlot> inventory)
    {
        Capacity = capacity;
        Backpack = inventory;
    }

    public void PickupItem(Item item, int quantity)
    {
        if (item.IsStackable)
        {
            Backpack[0].Add(item);
        }
        else
        {
            Backpack[0].Add(item);
        }
    }
}