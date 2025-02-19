using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public SceneReference ToScene;

    public void Start()
    {
        try
        {
            GameManager.LoadGame();
            print("Game loaded!");
            print(GameManager.GetRoomsExplored());
            SceneManager.LoadScene(ToScene.ScenePath);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}