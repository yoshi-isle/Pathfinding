#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoLoader
{
    private const string LastSceneKey = "AutoLoader_LastScene";

    static AutoLoader()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        string loaderScenePath = "Assets/Scenes/Loader.unity"; // Change this if needed

        if (state == PlayModeStateChange.ExitingEditMode) // Before entering Play mode
        {
            SceneSetup[] currentScenes = EditorSceneManager.GetSceneManagerSetup();
            if (currentScenes.Length > 0)
            {
                // Save the currently open scene(s)
                EditorPrefs.SetString(LastSceneKey, currentScenes[0].path);
            }

            // Switch to Loader scene if it's not already open
            if (EditorSceneManager.GetActiveScene().path != loaderScenePath)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(loaderScenePath);
            }
        }
        else if (state == PlayModeStateChange.EnteredEditMode) // After exiting Play mode
        {
            // Restore the previously open scene
            if (EditorPrefs.HasKey(LastSceneKey))
            {
                string lastScene = EditorPrefs.GetString(LastSceneKey);
                if (!string.IsNullOrEmpty(lastScene) && lastScene != loaderScenePath)
                {
                    EditorSceneManager.OpenScene(lastScene);
                }
            }
        }
    }
}
#endif