using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
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
                if (Run)
                {
                    currentPath.Pop();
                    if (currentPath.Count > 1) currentPath.Pop();
                }
                else
                {
                    currentPath.Pop();
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
                Vector2 gridLocation = WorldGrid.instance.WorldLocationToGrid(hit.point);

                // TODO - template click indicator
                GameObject clickIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
                clickIndicator.transform.position = new Vector3(gridLocation.x, 0.2f, gridLocation.y);
                clickIndicator.transform.localScale = new Vector3(0.4f, 6f, 0.4f);
                clickIndicator.GetComponent<Renderer>().material = clickIndicatorMaterial;
                Destroy(clickIndicator, 0.1f);

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

