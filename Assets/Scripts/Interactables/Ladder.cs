using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;

public class Ladder : Interactable
{
    public SceneReference ToScene;

    public override void Interact()
    {
        if (ToScene != null && !string.IsNullOrEmpty(ToScene.ScenePath))
        {
            SceneManager.LoadScene(ToScene.ScenePath);
        }
        else
        {
            Debug.LogError("No scene specified for the ladder or scene path is empty.");
        }
    }
}