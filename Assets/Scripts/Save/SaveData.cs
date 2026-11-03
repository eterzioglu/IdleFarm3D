using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int version = 1;
    public int gold = 0;
    public List<string> unlockedSectionIDs = new List<string>();
    public long lastSaveTime = 0;
}
