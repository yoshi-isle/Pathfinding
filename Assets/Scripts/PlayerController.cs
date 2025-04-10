using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool DrawGizmos;
    public Request request;
    public Vector2? targetTile = null;
    public Stack<Vector2> currentPath;
    public Vector2 GridLocation => new(transform.position.x, transform.position.z);
    public Interactable hoveredInteractable;
    public Interactable targetInteractable;
    public static event Action<Vector3> WorldClick;
    public Camera Camera;
    public PlayerState playerState;
    public bool Run = true;
    public int AttackCooldown = 0;
    public int CurrentAttackSpeed = 4;
    public Enemy AttackTarget;
    public Inventory inventory;
    public float currentHp = 100;
    public float maxHp = 100;
    [SerializeField] private List<Vector2> InteractLocationsRelative = new List<Vector2>
    {
        new Vector2(1, 0),
        new Vector2(-1, 0),
        new Vector2(0, 1),
        new Vector2(0, -1)
    };
    public enum PlayerState
    {
        Normal,
        Cutscene
    }

    void Start()
    {
        // Game event subscriptions
        TickCounter.Instance.OnTick += OnTick;
        Interactable.InteractableMouseHover += InteractableMouseHover;
        Interactable.InteractableMouseExit += InteractableMouseExit;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.PickupItem(ItemIndex.Items[1], 1);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Walkable"))
                {
                    WorldClick?.Invoke(Input.mousePosition);
                    Vector2 gridLocation = WorldGrid.instance.WorldLocationToGrid(hit.point);

                    if (WorldGrid.instance.InBoundsAndWalkable(gridLocation))
                    {
                        request = new WorldClickRequest(gridLocation.x, gridLocation.y);
                    }
                }
                else if (hoveredInteractable != null)
                {
                    hoveredInteractable.OnClick();
                    request = new InteractableClickRequest(hoveredInteractable);
                }
            }
        }
    }

    private void OnTick()
    {
        ProcessPreTickRequest();

        // Check if at interactable
        if (targetInteractable != null)
        {
            foreach (var location in targetInteractable.GetGridInteractionLocations())
            {
                if (GridLocation == location)
                {
                    print($"Reached an interact location for {targetInteractable.gameObject.name}");
                    targetInteractable.Interact();
                    if (targetInteractable is Enemy)
                    {
                        AttackTarget = (Enemy)targetInteractable;
                    }
                    targetInteractable = null;
                }
            }
        }

        if (GridLocation == targetTile)
        {
            targetTile = null;
        }

        // Move along path if we're not at the target
        if (targetTile != null)
        {
            // Impossible path
            if (currentPath.Count == 0)
            {
                targetTile = WorldGrid.instance.WorldLocationToGrid(transform.position);
            }

            if (currentPath != null && currentPath.Count > 1)
            {
                currentPath.Pop();

                if (Run)
                {
                    Vector2 nextTravelVector = currentPath.Count > 1 ? currentPath.ElementAt(1) - WorldGrid.instance.WorldLocationToGrid(transform.position) : Vector2.zero;

                    if (WorldGrid.instance.GetRunCollisionTravelRules.ContainsKey(nextTravelVector))
                    {
                        if (WorldGrid.instance.InBoundsAndWalkable(currentPath.ElementAt(1) - WorldGrid.instance.GetRunCollisionTravelRules[nextTravelVector]))
                        {
                            currentPath.Pop();
                        }
                    }
                }

                transform.position = new Vector3(currentPath.Peek().x, transform.position.y, currentPath.Peek().y);
            }
        }
    }

    private void ProcessPreTickRequest()
    {
        if (request == null) return;

        switch (playerState)
        {
            case PlayerState.Normal:
                if (request is WorldClickRequest worldClickRequest)
                {
                    targetTile = new Vector2(Mathf.Round(worldClickRequest.X), Mathf.Round(worldClickRequest.Y));
                    targetInteractable = null;
                    currentPath = AStarPathfinder.FindPath(GridLocation, (Vector2)targetTile);
                }
                else if (request is InteractableClickRequest interactableClickRequest)
                {
                    // TODO - Path to each. May not be efficient
                    var pathsToInteractable = new List<Stack<Vector2>>();
                    foreach (var possibleLocation in interactableClickRequest.clickedInteractable.GetGridInteractionLocations())
                    {
                        var path = AStarPathfinder.FindPath(GridLocation, possibleLocation);
                        if (path.Count() != 0)
                        {
                            pathsToInteractable.Add(path);
                        }
                    }
                    var shortestPath = pathsToInteractable.OrderBy(path => path.Count).FirstOrDefault();
                    if (shortestPath != null)
                    {
                        currentPath = shortestPath;
                        targetInteractable = interactableClickRequest.clickedInteractable;
                        targetTile = currentPath.Last();
                    }
                }
                break;
            case PlayerState.Cutscene:
                print("Player is in a cutscene, so the request is ignored.");
                break;
            default:
                break;
        }
        request = null;
    }

    private void InteractableMouseExit(Interactable interactable)
    {
        hoveredInteractable = null;
    }

    private void InteractableMouseHover(Interactable interactable)
    {
        hoveredInteractable = interactable;
    }

    public List<Vector3> GetWorldInteractionLocations()
    {
        var worldInteractionLocations = new List<Vector3>();
        foreach (var location in InteractLocationsRelative)
        {
            worldInteractionLocations.Add(new(location.x + transform.position.x, transform.position.y, location.y + transform.position.z));
        }
        return worldInteractionLocations;
    }

    void OnDrawGizmos()
    {
        if (targetTile == null)
        {
            return;
        }
        if (DrawGizmos)
        {
            Vector2 target = (Vector2)targetTile;
            if (currentPath == null) return;

            Gizmos.DrawSphere(new(target.x, 0, target.y), 0.2f);
            if (currentPath.Count != 0)
            {
                Gizmos.color = Color.magenta;

                foreach (var tile in currentPath)
                {
                    Gizmos.DrawSphere(new(tile.x, 0, tile.y), 0.2f);
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Application is quitting. Saving game...");
        GameManager.SavePlayerPosition(GridLocation);
        GameManager.SavePlayerInventory(inventory);
        GameManager.SaveGame();
    }
}

