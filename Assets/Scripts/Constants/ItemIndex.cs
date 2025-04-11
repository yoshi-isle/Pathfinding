using System.Collections.Generic;

public static class ItemIndex
{
    public static Dictionary<int, Item> Items => new Dictionary<int, Item>
    {
        {1, new Item("Wooden Stick", 10)},
        {2, new Item("Apple", 6)},
        {3, new Item("Iron Axe", 1)},
        {4, new Item("Healing Potion", 1)},
    };
}