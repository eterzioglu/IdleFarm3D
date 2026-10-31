using UnityEngine;
using UnityEngine.InputSystem;

public class ClickInputHandler : MonoBehaviour
{
    void Update()
    {
       if (Mouse.current == null) { GameLogger.Log("Mouse.current null", LogLevel.Error); return; }
       if (!Mouse.current.leftButton.wasPressedThisFrame) return;
   
       GameLogger.Log("1) Click detected");
   
       if (Camera.main == null) { GameLogger.Log("Camera.main null", LogLevel.Error); return; }
   
       Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
   
       if (!Physics.Raycast(ray, out RaycastHit hit))
       {
           GameLogger.Log("2) Ray hit nothing");
           return;
       }
   
       GameLogger.Log($"2) Ray hit: {hit.collider.name}");
   
       if (hit.collider.TryGetComponent(out WorkTask task))
       {
           GameLogger.Log($"3) WorkTask found, TryStart = {task.TryStart()}");
       }
       else
       {
           GameLogger.Log("3) No WorkTask on hit object");
       }
    }
}
   