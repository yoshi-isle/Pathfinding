using UnityEngine;
using UnityEngine.SceneManagement;

public class Ladder : Interactable
{
    public SceneReference ToScene;
    public Vector3 ToLocation;

    public override void Interact()
    {
        if (ToScene != null && !string.IsNullOrEmpty(ToScene.ScenePath))
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(ToScene.ScenePath);
        }
        else
        {
            Debug.LogError("No scene specified for the ladder or scene path is empty.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = ToLocation;
        }
        else
        {
            Debug.LogError("Player object not found in the scene.");
        }
    }
}