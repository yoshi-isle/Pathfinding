using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool DrawGizmos;
    public Request request;
    public Vector2 targetTile;
    public List<Vector2> currentPath;
    public PlayerState playerState;
    public Vector2 GridLocation => new(transform.position.x, transform.position.z);

    public enum PlayerState
    {
        Idle,
        Walking
    }

    void Start()
    {
        playerState = PlayerState.Idle;
        targetTile = WorldGrid.instance.WorldLocationToGrid(transform.position);
        GameManager.Instance.OnTick += OnTick;
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

    void OnTick()
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
                currentPath.RemoveAt(0);
                transform.position = new Vector3(currentPath.First().x, transform.position.y, currentPath.First().y);
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
                    currentPath = AStarPathfinder.instance.FindPath(GridLocation, targetTile);
                }
                request = null;
                break;
            case PlayerState.Walking:
                if (request is WorldClickRequest worldClickRequest2)
                {
                    targetTile = new Vector2(Mathf.Round(worldClickRequest2.X), Mathf.Round(worldClickRequest2.Y));
                    currentPath = AStarPathfinder.instance.FindPath(GridLocation, targetTile);
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
                Vector2 gridLocation = WorldGrid.instance.WorldLocationToGrid(hit.point);
                if (WorldGrid.instance.InBoundsAndWalkable(gridLocation))
                {
                    request = new WorldClickRequest(gridLocation.x, gridLocation.y);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (DrawGizmos)
        {
            Gizmos.DrawCube(new(targetTile.x, 0, targetTile.y), new(1, 1, 1));

            if (currentPath.Count != 0)
            {
                Gizmos.color = Color.magenta;

                foreach (var tile in currentPath)
                {
                    Gizmos.DrawCube(new(tile.x, 0, tile.y), new(0.9f, 0.9f, 0.9f));
                }
            }
        }
    }
}

