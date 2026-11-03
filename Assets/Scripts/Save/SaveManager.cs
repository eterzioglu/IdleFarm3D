using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[DefaultExecutionOrder(-100)]
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public SaveData SaveData { get; private set; }
    public bool HasSave { get; private set; }
    string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        Instance = this;
        Load();
    }

    private void Start()
    {
        GameLogger.Log($"SaveManager initialized. Save path: {SavePath}. Has save: {HasSave}. Current gold: {SaveData.gold}", LogLevel.Info);
    }

    private void Load()
    {
        SaveData = new SaveData();
        if (!File.Exists(SavePath)) return;

        try
        {
            string json = File.ReadAllText(SavePath);
            SaveData = JsonUtility.FromJson<SaveData>(json);
            HasSave = true;
        }
        catch (Exception e)
        {
            GameLogger.Log("Save load failed: " + e.Message, LogLevel.Error);
            SaveData = new SaveData();
        }
    }

    private void Save()
    {
        if (GoldManager.Instance == null) return;

        try
        {
            SaveData.lastSaveTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            SaveData.gold = GoldManager.Instance.Gold;
            SaveData.unlockedSectionIDs.Clear();
            foreach (var s in FindObjectsByType<Section>(FindObjectsSortMode.None))
            {
                if (s.IsUnlocked)
                {
                    SaveData.unlockedSectionIDs.Add(s.SectionId);
                }
            }

            string json = JsonUtility.ToJson(SaveData, true);
            File.WriteAllText(SavePath, json);
            
        }
        catch (Exception e)
        {
            GameLogger.Log($"Failed to save data: {e.Message}", LogLevel.Error);
        }
    }

    public void ResetSave()
    {
        SaveData = new SaveData();
        HasSave = false;
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            Save();
    }

    private void OnApplicationQuit() => Save();
}
