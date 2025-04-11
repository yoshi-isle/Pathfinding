using System;
using System.IO;
using UnityEngine;

public static class GameManager
{
    private static readonly string saveFilePath = Path.Combine(Application.dataPath, "Scripts", "PlayerData.json");

    [Serializable]
    public class PlayerData
    {
        public Vector2 GridLocation;
        public Inventory inventory;
        public PlayerData()
        {
            GridLocation = Vector2.zero;
            inventory = new Inventory(12);
        }
    }

    private static PlayerData currentPlayerData;

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

    public static void SavePlayerInventory(Inventory inventory)
    {
        currentPlayerData.inventory = inventory;
        SaveGame();
    }

    public static void SavePlayerPosition(Vector2 gridLocation)
    {
        currentPlayerData.GridLocation = gridLocation;
    }

    public static Vector2 LoadPlayerPosition()
    {
        return currentPlayerData.GridLocation;
    }

    public static Inventory LoadPlayerInventory()
    {
        var inventory = new Inventory(currentPlayerData.inventory.Capacity, currentPlayerData.inventory.Backpack);
        return inventory;
    }
}