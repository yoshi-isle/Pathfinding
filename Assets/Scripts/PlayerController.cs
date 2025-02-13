using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool DrawGizmos;
    public Request request;
    public Vector2 targetTile;
    public Stack<Vector2> currentPath;
    public PlayerState playerState;
    public Vector2 GridLocation => new(transform.position.x, transform.position.z);
    public Material clickIndicatorMaterial;
    public Interactable hoveredInteractable;
    public Interactable targetInteractable;
    public static event Action<Vector3> WorldClick;

    public bool Run;
    public enum PlayerState
    {
        Idle,
        Walking
    }

    void Start()
    {
        playerState = PlayerState.Idle;
        targetTile = WorldGrid.instance.WorldLocationToGrid(transform.position);

        // Game event subscriptions
        GameManager.Instance.OnTick += OnTick;
        Interactable.InteractableMouseHover += InteractableMouseHover;
        Interactable.InteractableMouseExit += InteractableMouseExit;
    }

    void Update()
    {
        // Listen for inputs
        switch (playerState)
        {
            case PlayerState.Idle:
                ListenForWorldClicks();
                break;
            case PlayerState.Walking:
                ListenForWorldClicks();
                break;
            default:
                break;
        }
    }

    void HandleWalk()
    {
        // Move along path if we're not at the target
        if (GridLocation != targetTile)
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
                    /*
                    Analyze the "travel" vector
                    Determine if valid run action (dictonary)
                    Travel vector -> maps to -> fail points
                    */
                    //TODO - Move this
                    Vector2 nextTravelVector = currentPath.Count > 1 ? currentPath.ElementAt(1) - WorldGrid.instance.WorldLocationToGrid(transform.position) : Vector2.zero;
                    Dictionary<Vector2, Vector2> travelMap = new() {
                        { new Vector2(-2, 2), new Vector2(-1, 1) },
                        { new Vector2(-2, 1), new Vector2(-1, 1) },
                        { new Vector2(-2, 0), new Vector2(-1, 0) },
                        { new Vector2(-2, -1), new Vector2(-1, -1) },
                        { new Vector2(-2, -2), new Vector2(-1, -1) },
                        { new Vector2(-1, 2), new Vector2(-1, 1) },
                        { new Vector2(0, 2), new Vector2(0, 1) },
                        { new Vector2(1, 2), new Vector2(1, 1) },
                        { new Vector2(2, 2), new Vector2(1, 1) },
                        { new Vector2(2, 1), new Vector2(1, 1) },
                        { new Vector2(2, 0), new Vector2(1, 0) },
                        { new Vector2(2, -1), new Vector2(1, -1) },
                        { new Vector2(2, -2), new Vector2(1, -1) },
                        { new Vector2(1, -2), new Vector2(1, -1) },
                        { new Vector2(0, -2), new Vector2(0, -1) },
                        { new Vector2(-1, -2), new Vector2(-1, -1) }
                    };

                    if (travelMap.ContainsKey(nextTravelVector))
                    {
                        if (WorldGrid.instance.InBoundsAndWalkable(currentPath.ElementAt(1) - travelMap[nextTravelVector]))
                        {
                            currentPath.Pop();
                        }
                    }
                }

                transform.position = new Vector3(currentPath.Peek().x, transform.position.y, currentPath.Peek().y);
            }
        }
        else
        {
            playerState = PlayerState.Idle;
            currentPath.Clear();
        }
    }

    private void ProcessPreTickStateChangeRequest()
    {
        if (request == null) return;

        switch (playerState)
        {
            case PlayerState.Idle:
                if (request is WorldClickRequest worldClickRequest)
                {
                    playerState = PlayerState.Walking;
                    targetTile = new Vector2(Mathf.Round(worldClickRequest.X), Mathf.Round(worldClickRequest.Y));
                    currentPath = AStarPathfinder.FindPath(GridLocation, targetTile);
                }
                request = null;
                break;
            case PlayerState.Walking:
                if (request is WorldClickRequest worldClickRequest2)
                {
                    targetTile = new Vector2(Mathf.Round(worldClickRequest2.X), Mathf.Round(worldClickRequest2.Y));
                    currentPath = AStarPathfinder.FindPath(GridLocation, targetTile);
                }
                request = null;
                break;
            default:
                break;
        }
    }

    private void ListenForWorldClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
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
                    targetInteractable = hoveredInteractable;
                }

            }
            else
            {
                print("Something else");
            }
        }
    }

    private void OnTick()
    {
        // Process any state change requests
        ProcessPreTickStateChangeRequest();

        // Process actions on the tick
        switch (playerState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Walking:
                HandleWalk();
                break;
            default:
                break;
        }
    }

    private void InteractableMouseExit(Interactable interactable)
    {
        hoveredInteractable = null;
    }

    private void InteractableMouseHover(Interactable interactable)
    {
        hoveredInteractable = interactable;
    }

    void OnDrawGizmos()
    {
        if (DrawGizmos)
        {
            if (currentPath == null) return;

            Gizmos.DrawSphere(new(targetTile.x, 0, targetTile.y), 0.2f);
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
}

