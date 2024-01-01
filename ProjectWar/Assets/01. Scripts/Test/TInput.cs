using System;
using UnityEngine;

public class TInput : MonoBehaviour
{
    [SerializeField] InputReader inputReader = null;

    private void Awake()
    {
        inputReader.OnNumberKeyPressed += HandleNumberKeyPressed;
        inputReader.OnLeftClicked += HandleLeftClicked;
        inputReader.OnRightClicked += HandleRightClicked;
    }

    private void HandleRightClicked(bool value)
    {
        Debug.Log($"Right Clicked : {value}");
    }

    private void HandleLeftClicked(bool value)
    {
        Debug.Log($"Left Clicked : {value}");
    }

    private void HandleNumberKeyPressed(int value)
    {
        Debug.Log($"Numbers Clicked : {value}");
    }
}
