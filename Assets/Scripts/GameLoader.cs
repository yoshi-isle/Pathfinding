using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public SceneReference ToScene;
    public GameObject playerPrefab;
    public GameObject playerGraphicsPrefab;
    public GameObject cameraPrefab;

    public void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        GameManager.LoadGame();
        print("Game loaded!");
        SceneManager.LoadScene(ToScene.ScenePath);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var playerGridPosition = GameManager.LoadPlayerPosition();
        var spawnPoint = new Vector3(playerGridPosition.x, 0, playerGridPosition.y);
        var player = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
        var playerGfx = Instantiate(playerGraphicsPrefab, spawnPoint, Quaternion.identity);
        var camera = Instantiate(cameraPrefab, spawnPoint, Quaternion.identity);
        playerGfx.GetComponent<PlayerGraphicsController>().follow = player.transform;
        camera.GetComponent<CameraController>().target = playerGfx.transform;
        player.GetComponent<PlayerController>().Camera = camera.GetComponent<Camera>();
        player.GetComponent<PlayerController>().inventory = GameManager.LoadPlayerInventory();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}