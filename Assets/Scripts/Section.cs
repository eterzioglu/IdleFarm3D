using UnityEngine;
using System;
using System.Collections.Generic;

public class Section : MonoBehaviour
{
    [SerializeField] private WorkTask workTaskPrefab;
    [SerializeField] private int unlockCost = 100;
    [SerializeField] private Material lockedMaterial;
    [SerializeField] private Material unlockedMaterial;
    private bool isUnlocked = false;

    event Action<Section> OnUnlocked;

    private bool TryUnlock()
    {
        if (isUnlocked) return false;

        if (GoldManager.Instance.TrySpendGold(unlockCost))
        {
            isUnlocked = true;
            GetComponent<Renderer>().material = unlockedMaterial;
            GameLogger.Log($"Section unlocked! Cost: {unlockCost} gold.", LogLevel.Info);
            OnUnlocked?.Invoke(this);
            return true;
        }
        else
        {
            GameLogger.Log("Not enough gold to unlock this section.", LogLevel.Warning);
        }
        return false;
    }
}
