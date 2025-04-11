using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class SceneReference
{
    [SerializeField]
    private SceneAsset sceneAsset;

    public string ScenePath
    {
        get
        {
            if (sceneAsset == null)
            {
                return null;
            }
            return AssetDatabase.GetAssetPath(sceneAsset);
        }
    }
}
