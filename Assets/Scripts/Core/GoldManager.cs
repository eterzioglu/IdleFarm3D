using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance{ get; private set; }

    public int Gold{ get; private set; }
    public event Action<int> OnGoldChanged;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        Gold = SaveManager.Instance.HasSave ? SaveManager.Instance.SaveData.gold : 0;
    }


    public void AddGold(int amount)
    {
        if(amount <= 0) return;

        Gold+=amount;
        OnGoldChanged?.Invoke(Gold);
    }

    public bool TrySpendGold(int amount)
    {
        if(amount <= 0 || Gold < amount) 
        {
            GameLogger.Log("Not enough gold to spend or invalid amount.");
            return false;
        }

        Gold-=amount;
        OnGoldChanged?.Invoke(Gold);
        return true;
    }

    private void Update()
    {
        KeyInput();
    }


#if UNITY_EDITOR
    private void KeyInput()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            AddGold(50);
            GameLogger.Log("Added 50 gold. Current gold: " + Gold);
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            if(!TrySpendGold(25)) return;
            GameLogger.Log("Tried to spend 25 gold. Current gold: " + Gold);
        }

        if(Keyboard.current.rKey.wasPressedThisFrame)
        {
            SaveManager.Instance.ResetSave();
            Gold = 0;
            OnGoldChanged?.Invoke(Gold);
            GameLogger.Log("Reset save data. Current gold: " + Gold);
        }
    }
#endif
}
