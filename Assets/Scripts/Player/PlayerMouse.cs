using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouse
{
    public static Vector2 pos { get
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            float distance;
            if (new Plane(Vector3.forward, Vector3.zero).Raycast(ray, out distance))
            {
                return ray.GetPoint(distance);
            }
            throw new System.Exception("Mouse don't work");
        }
    }
}
