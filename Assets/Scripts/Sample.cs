// void Start()
// {
//     // Check save data to see if this chest was already opened
//     if (SaveSystem.Instance.IsObjectStateSet(objectID, "opened"))
//     {
//         OpenChest(); // Apply saved state
//     }
// }

// {
//     "scene": "cave",
//   "player_position": { "x": 10, "y": 5 },
//   "inventory": [...],
//   "interactables": {
//         "CaveChest_001": { "opened": true },
//     "SecretDoor_002": { "unlocked": true }
//     }
// }

// /*
// 1. Auto-Generate a Unique ID in the Editor
// You can generate a unique ID automatically when the object is placed in the scene. This ensures each object has a unique, persistent ID without you having to assign them manually.

// Option 1: Use a GUID (Editor Only)
// */
// using UnityEngine;
// using System;

// [ExecuteInEditMode]
// public class TreasureChest : MonoBehaviour
// {
//     [SerializeField] private string objectID;  // Serialized so it's saved in the scene

//     private void Awake()
//     {
//         if (string.IsNullOrEmpty(objectID))
//         {
//             objectID = Guid.NewGuid().ToString();  // Generate unique ID only once
// #if UNITY_EDITOR
//             UnityEditor.EditorUtility.SetDirty(this);  // Mark object as changed
// #endif
//         }
//     }

//     public string GetID() => objectID;
// }
