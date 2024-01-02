using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(menuName = "SO/Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    private Controls controls;

    public event Action<float> OnArrowKeyPressed;
    public event Action<bool> OnLeftClicked;
    public event Action<bool> OnRightClicked;
    public event Action<int> OnNumberKeyPressed;
    public event Action OnSkill1Pressed;
    public event Action OnSkill2Pressed;
    public event Action OnToggleKeyPressed;

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

    public void OnSkill1(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSkill1Pressed?.Invoke();
    }

    public void OnSkill2(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnSkill2Pressed?.Invoke();
    }

    public void OnToggle(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnToggleKeyPressed?.Invoke();
    }

    public void OnArrow(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnArrowKeyPressed?.Invoke(context.ReadValue<float>());
    }
}
