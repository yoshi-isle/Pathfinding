using System;

[Serializable]
public class Item
{
    public string Name;
    public int MaxQuantity;
    public bool IsStackable => MaxQuantity > 1;

    public Item(string name, int maxQuantity)
    {
        Name = name;
        MaxQuantity = maxQuantity;
    }
}