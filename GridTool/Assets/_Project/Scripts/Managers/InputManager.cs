using UnityEngine;
using UnityUtils;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private LayerMask groundLayer;
    public Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.groundLayer);
        return raycastHit.point;
    }
}
