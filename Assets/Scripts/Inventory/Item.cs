using System;

[Serializable]
public abstract class Item
{
    public string Name { get; set; }
}

[Serializable]
public class SampleItem : Item
{

}