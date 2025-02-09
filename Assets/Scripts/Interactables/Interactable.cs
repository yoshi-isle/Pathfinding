using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Vector3 InteractLocation;
    public int InteractRadius;
    public string InteractName;
    public string InteractDescription;
    public static event Action<Interactable> InteractableMouseHover;
    public static event Action<Interactable> InteractableMouseExit;

    void Update()
    {

    }

    void OnMouseOver()
    {
        InteractableMouseHover?.Invoke(this);
    }

    void OnMouseExit()
    {
        InteractableMouseExit?.Invoke(this);
    }

}