using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Interactable
{
    public bool AggroTowardsPlayer = false;
    public EnemyState CurrentState = EnemyState.Patrol;
    public enum EnemyState
    {
        Patrol,
        Aggro
    }
    GameObject player;
    public bool ShowHpBar = false;
    public int AttackRange = 0;
    public Vector2 GridLocation => new(transform.position.x, transform.position.z);

    public List<Vector2> GetTilesInScoutRange()
    {
        List<Vector2> tiles = new List<Vector2>();
        Vector2 gridLocation = GridLocation;

        // Add the initial tile (range 0 = current tile)
        tiles.Add(gridLocation);

        for (int x = -AttackRange; x <= AttackRange; x++)
        {
            for (int y = -AttackRange; y <= AttackRange; y++)
            {
                if (x == 0 && y == 0) continue;

                tiles.Add(new Vector2(gridLocation.x + x, gridLocation.y + y));
            }
        }

        return tiles;
    }

    public void OnTick()
    {
        print("Checking attack range");
        // TODO - Inefficient - use physics or precise colliders(?)

        bool found = false;
        foreach (Vector2 tile in GetTilesInScoutRange())
        {
            if (player.GetComponent<PlayerController>().GridLocation == tile)
            {
                found = true;
            }
        }

        if (found)
        {
            print("Player is in range. now im angy");
            SetAggressive();
        }
        else
        {
            print("Player is gone. not angy anymore");
            SetPatrol();
        }
    }

    public void Start()
    {
        //TODO
        player = GameObject.FindGameObjectWithTag("Player");

        TickCounter.Instance.OnTick += OnTick;
    }
    public override void Interact()
    {
        SetAggressive();
        if (AggroTowardsPlayer)
        {
            if (player != null)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }

    public void SetAggressive()
    {
        ShowHpBar = true;
        AggroTowardsPlayer = true;
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }
    }

    private void SetPatrol()
    {
        ShowHpBar = false;
        AggroTowardsPlayer = true;
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white;
        }
    }

    void OnDrawGizmos()
    {
        foreach (var tile in GetTilesInScoutRange())
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(new(tile.x,0,tile.y), Vector3.one*0.7f);
        }
    }

    void OnGUI()
    {
        if (ShowHpBar)
        {
            Camera camera = FindAnyObjectByType<Camera>();
            Vector3 hpBarPosition = camera.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
            float hpBarWidth = 30;
            float hpBarHeight = 5;
            float currentHp = 100;
            float maxHp = 100;
            

            // Draw background (depleted health)
            GUI.color = Color.red;
            GUI.DrawTexture(new Rect(hpBarPosition.x - hpBarWidth / 2, Screen.height - hpBarPosition.y - hpBarHeight / 2, hpBarWidth, hpBarHeight), Texture2D.whiteTexture);

            // Draw foreground (current health)
            GUI.color = Color.green;
            GUI.DrawTexture(new Rect(hpBarPosition.x - hpBarWidth / 2, Screen.height - hpBarPosition.y - hpBarHeight / 2, hpBarWidth * (currentHp / maxHp), hpBarHeight), Texture2D.whiteTexture);
        }
    }
}