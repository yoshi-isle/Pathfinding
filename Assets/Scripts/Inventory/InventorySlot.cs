using System;

#nullable enable

[Serializable]
public class InventorySlot
{
    public int Index;
    public Item? Item;
    public int Quantity;

    public InventorySlot(int index)
    {
        Index = index;
        Item = null;
        Quantity = 0;
    }

    public InventorySlot(int index, Item item, int quantity)
    {
        Index = index;
        Item = item;
        Quantity = quantity;
    }

    public InventorySlot() 
    {
        Index = -1;
        Item = null;
        Quantity = 0;
    }

    public void Add(Item item)
    {
        Item = item;
    }

    public void RemoveItem() // Changed from private to public
    {
        Item = null;
    }

    public void ChangeQuantity(int quantity) // Changed from private to public
    {
        Quantity = quantity;
    }
}