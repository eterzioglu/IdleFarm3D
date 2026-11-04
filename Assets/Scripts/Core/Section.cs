using UnityEngine;
using System;
using System.Collections.Generic;

public class Section : MonoBehaviour
{
    [SerializeField] private string sectionId = "0";
    [SerializeField] private int unlockCost = 100;
    [SerializeField] private bool isUnlockedByDefault = false;
    [SerializeField] private Material lockedMaterial;
    [SerializeField] private Material unlockedMaterial;

    public string SectionId => sectionId;
    public bool IsUnlocked { get; private set; }
    public event Action<Section> OnUnlocked;
    public int UnlockCost => unlockCost;

    private WorkTask workTask;
    private Renderer sectionRenderer;

    private void Awake()
    {
        IsUnlocked = isUnlockedByDefault;
        sectionRenderer = GetComponent<Renderer>();
        workTask = GetComponent<WorkTask>();

        if (SaveManager.Instance.HasSave && isUnlockedByDefault == false)
            IsUnlocked = SaveManager.Instance.SaveData.unlockedSectionIDs.Contains(sectionId);
        else
            IsUnlocked = isUnlockedByDefault;
    }

    private void Start()
    {
        ApplyMaterial();
    }


    public bool TryUnlock()
    {
        if (IsUnlocked) return false;

        if (GoldManager.Instance.TrySpendGold(unlockCost))
        {
            IsUnlocked = true;
            ApplyMaterial();
            GameLogger.Log($"Section unlocked! Cost: {unlockCost} gold.", LogLevel.Info);
            OnUnlocked?.Invoke(this);
            GameLogger.Log($"Section unlocked successfully. Current gold: {GoldManager.Instance.Gold}", LogLevel.Info);
            return true;
        }
        else
        {
            GameLogger.Log("Not enough gold to unlock this section.", LogLevel.Warning);
        }
        return false;
    }

    public void OnClick()
    {
        if (!IsUnlocked)
        {
            TryUnlock();
        }
        else
        {
            if (workTask != null)
            {
                if (!workTask.IsRunning)
                {
                    workTask.TryStart();
                }
                else
                {
                    GameLogger.Log("Task is already running.", LogLevel.Warning);
                }
            }
            else
            {
                GameLogger.Log("No WorkTask component found on this section.", LogLevel.Warning);
            }
        }
    }

    private void ApplyMaterial()
    {
        if (sectionRenderer != null)
        {
            sectionRenderer.material = IsUnlocked ? unlockedMaterial : lockedMaterial;
        }
    }

    #if UNITY_EDITOR
    public void SetIdForTesting(string id)
    {
        sectionId = id;
    }

    public void SetUnlockCostForTesting(int cost)
    {
        unlockCost = cost;
    }

    public bool TryUnlockForTesting(GoldManager goldManager)
    {
        if (IsUnlocked) return false;

        if (goldManager.TrySpendGold(unlockCost))
        {
            IsUnlocked = true;
            ApplyMaterial();
            GameLogger.Log($"Section unlocked! Cost: {unlockCost} gold.", LogLevel.Info);
            OnUnlocked?.Invoke(this);
            GameLogger.Log($"Section unlocked successfully. Current gold: {goldManager.Gold}", LogLevel.Info);
            return true;
        }
        else
        {
            GameLogger.Log("Not enough gold to unlock this section.", LogLevel.Warning);
        }
        return false;
    }
    #endif
}
