using System;

public class WorldClickRequest : Request
{
    public float X { get; set; }
    public float Y { get; set; }
    public static event Action<WorldClickRequest> WorldClickEvent;

    public WorldClickRequest(float x, float y)
    {
        X = x;
        Y = y;
        WorldClickEvent.Invoke(this);
    }
}