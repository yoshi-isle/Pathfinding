using System.Linq;
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
        if (pc == null)
        {
            pc = FindFirstObjectByType<PlayerController>();
        }
        if (pc == null)
        {
            return;
        }
        string request_string = "None";
        if (pc.request != null)
        {
            if (pc.request is WorldClickRequest wcr)
            {
                request_string = $"WORLD CLICK ({wcr.X}, {wcr.Y})";
            }
            if (pc.request is InteractableClickRequest icr)
            {
                request_string = $"INTERACTABLE CLICK ({icr.clickedInteractable.gameObject.name})";
            }
        }

        Vector2 player_tile = WorldGrid.instance.WorldLocationToGrid(pc.gameObject.transform.position);
        text.text = $@"
        Player Tile: {player_tile.x}, {player_tile.y}
        Player State: {pc.playerState}
        Tick Request: {request_string}
        Path Length: {(pc.currentPath != null && pc.currentPath.Count > 0 ? pc.currentPath.Count : "N/A")}
        Target Tile: {pc.targetTile}
        Hovered Interactable: {pc.hoveredInteractable}
        Target Interactable: {pc.targetInteractable}
        Attack Target: {pc.AttackTarget}
        Attack Cooldown: {pc.AttackCooldown}
        Run Mode: {pc.Run}
        Inventory Capacity: {pc.inventory.Capacity}
        ";

    }

    public void OnClick()
    {
        ShowDebugInfo = !ShowDebugInfo;
        image.enabled = ShowDebugInfo;
        text.enabled = ShowDebugInfo;
    }
}