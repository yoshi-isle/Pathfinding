using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugInfo : MonoBehaviour
{
    public bool ShowDebugInfo = true;
    public Image image;
    public TMP_Text text;
    public PlayerController pc;

    void Start()
    {
        pc = FindFirstObjectByType<PlayerController>();
    }
    void Update()
    {
        string request_string = "None";
        if (pc.request != null)
        {
            if (pc.request is WorldClickRequest)
            {
                var wcr = (WorldClickRequest)pc.request;
                request_string = $"WORLD CLICK ({wcr.X}, {wcr.Y})";
            }
        }

        Vector2 player_tile = WorldGrid.instance.WorldLocationToGrid(pc.gameObject.transform.position);
        text.text = $@"
        Player Tile: {player_tile.x}, {player_tile.y}
        Player State: {pc.playerState}
        Tick Request: {request_string}
        Path Length: {(pc.currentPath != null && pc.currentPath.Count > 0 ? pc.currentPath.Count : "N/A")}
        Target Tile: {pc.targetTile}
        ";

    }

    public void OnClick()
    {
        ShowDebugInfo = !ShowDebugInfo;
        image.enabled = ShowDebugInfo;
        text.enabled = ShowDebugInfo;
    }
}