using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(menuName = "SO/Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    private Controls controls;

    public event Action<int> OnNumberKeyPressed;
    public event Action<bool> OnLeftClicked;
    public event Action<bool> OnRightClicked;

    public Vector2 MousePosition { get; private set; } = Vector2.zero;

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }

        controls.Player.Enable();
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnLeftClicked?.Invoke(true);
        else if(context.canceled)
            OnLeftClicked?.Invoke(false);
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnRightClicked?.Invoke(true);
        else if(context.canceled)
            OnRightClicked?.Invoke(false);
    }

    public void OnNumbers(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnNumberKeyPressed?.Invoke((int)context.ReadValue<float>());
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }
}