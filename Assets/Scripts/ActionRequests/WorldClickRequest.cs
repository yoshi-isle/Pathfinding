using UnityEngine;

public class WorldClickRequest : Request
{
    public float X { get; set; }
    public float Y { get; set; }

    public WorldClickRequest(float x, float y)
    {
        X = x;
        Y = y;
    }
}