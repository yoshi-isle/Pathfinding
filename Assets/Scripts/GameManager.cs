using System;
using System.IO;
using UnityEngine;

public static class GameManager
{
    private static readonly string saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");

    [Serializable]
    public class PlayerData
    {
        public int NumberOfRoomsExplored;
        public PlayerData()
        {
            NumberOfRoomsExplored = 0;
        }
    }

    private static PlayerData currentPlayerData = new PlayerData();

    public static void SaveGame()
    {
        try
        {
            string saveData = JsonUtility.ToJson(currentPlayerData);
            File.WriteAllText(saveFilePath, saveData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }

    public static void LoadGame()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string loadedData = File.ReadAllText(saveFilePath);
                currentPlayerData = JsonUtility.FromJson<PlayerData>(loadedData);
            }
            else
            {
                currentPlayerData = new PlayerData();
                SaveGame();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
            currentPlayerData = new PlayerData();
        }
    }

    public static void DeleteSaveFile()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
                currentPlayerData = new PlayerData();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to delete save file: {e.Message}");
        }
    }

    public static void IncrementRoomsExplored()
    {
        currentPlayerData.NumberOfRoomsExplored++;
    }

    public static int GetRoomsExplored()
    {
        return currentPlayerData.NumberOfRoomsExplored;
    }
}