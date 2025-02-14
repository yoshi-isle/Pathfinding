using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // TODO - Default to 4 directions
    [SerializeField] private List<Vector2> InteractLocationsRelative;
    public int InteractRadius;
    public string InteractName;
    public string InteractDescription;
    public static event Action<Interactable> InteractableMouseHover;
    public static event Action<Interactable> InteractableMouseExit;

    public void OnClick()
    {
        print($"I've been clicked {gameObject.name}");
    }

    void Update()
    {

    }

    public virtual void Interact()
    {
        print($"Interacting with {InteractName} ({InteractDescription})");
    }

    void OnMouseOver()
    {
        InteractableMouseHover?.Invoke(this);
    }

    void OnMouseExit()
    {
        InteractableMouseExit?.Invoke(this);
    }

    public List<Vector2> GetGridInteractionLocations()
    {
        var worldInteractionLocations = new List<Vector2>();
        foreach (var location in InteractLocationsRelative)
        {
            worldInteractionLocations.Add(new(location.x + transform.position.x, location.y + transform.position.z));
        }
        return worldInteractionLocations;
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
        foreach (var location in GetWorldInteractionLocations())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(location, Vector3.one * 0.7f);
        }
    }

}