using UnityEngine;
using UnityEngine.InputSystem;

public class ClickInputHandler : MonoBehaviour
{
    void Update()
    {
       if (Mouse.current == null) return;
       if (!Mouse.current.leftButton.wasPressedThisFrame) return;

   
       if (Camera.main == null) return;
   
       Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
   
       if (!Physics.Raycast(ray, out RaycastHit hit))
       {
           GameLogger.Log("2) Ray hit nothing");
           return;
       }
   
       if (hit.collider.TryGetComponent(out Section section))
       {
           GameLogger.Log("2) Section clicked");
           section.OnClick();
       }
       else
       {
           GameLogger.Log("2) No Section on hit object");
       }
    }
}
   