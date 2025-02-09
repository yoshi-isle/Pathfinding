using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class MouseToolTips : MonoBehaviour
{
    public RectTransform toolTipRectTransform;
    public TMP_Text toolTipText;
    public Vector3 offset;

    void Start()
    {
        Interactable.InteractableMouseHover += InteractableMouseHover;
        Interactable.InteractableMouseExit += InteractableMouseExit;
    }

    void Update()
    {
        transform.position = Input.mousePosition + offset;
    }

    private void InteractableMouseHover(Interactable interactable)
    {
        UpdateToolTip($"{interactable.InteractDescription} {interactable.InteractName}");
    }

    private void InteractableMouseExit(Interactable interactable)
    {
        UpdateToolTip($"");
    }

    private void UpdateToolTip(string text)
    {
        toolTipText.text = text;
        Canvas.ForceUpdateCanvases();
        toolTipRectTransform.sizeDelta = new Vector2(toolTipText.preferredWidth, toolTipRectTransform.sizeDelta.y);
    }
}