using UnityEngine;

public class WorkTask : MonoBehaviour
{
    [SerializeField] int reward = 10;
    [SerializeField] float duration = 3f;

    public bool IsRunning { get; private set; }
    public float Progress { get; private set; }

    float timer;

    public bool TryStart()
    {
        if (IsRunning) return false;

        IsRunning = true;
        timer = 0f;
        Progress = 0f;
        TaskManager.Instance.Register(this);
        GameLogger.Log($"Task started. Duration: {duration} seconds, Reward: {reward} gold.", LogLevel.Info);
        return true;
    }

    public void Tick(float deltaTime)
    {
        timer += deltaTime;
        Progress = Mathf.Clamp01(timer / duration);

        if (timer >= duration)
            Complete();
    }

    void Complete()
    {
        GoldManager.Instance.AddGold(reward);
        GameLogger.Log($"Task completed! Rewarded {reward} gold.", LogLevel.Info);
        Stop();
    }

    void Stop()
    {
        IsRunning = false;
        Progress = 0f;
        TaskManager.Instance.Unregister(this);
    }

    void OnDisable()
    {
        if (IsRunning) Stop();
    }
}
