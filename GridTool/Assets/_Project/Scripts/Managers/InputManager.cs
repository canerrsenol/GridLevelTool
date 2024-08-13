using System;
using UnityEngine;

public class InputManager : UnityUtils.Singleton<InputManager>
{
    public event Action<Vector2> OnClick; 
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick?.Invoke(Input.mousePosition);
        }
    }
}
