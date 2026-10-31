using UnityEngine;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }
    readonly List<WorkTask> activeTasks = new List<WorkTask>();

    void Awake() => Instance = this;

    public void Register(WorkTask task)   => activeTasks.Add(task);
    public void Unregister(WorkTask task) => activeTasks.Remove(task);

    void Update()
    {
        for (int i = activeTasks.Count - 1; i >= 0; i--)
            activeTasks[i].Tick(Time.deltaTime);
    }
}
