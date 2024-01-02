using System;
using UnityEngine;

public class PlayerInput : PlayerComponent
{
    [SerializeField] InputReader inputReader;

    private int currentLine = 1;

    public override void Init(Player player)
    {
        base.Init(player);

        if(player.IsOwner == false)
            return;

        inputReader.OnNumberKeyPressed += OnNumberKeyPressedHandle;
        inputReader.OnArrowKeyPressed += OnArrowKeyPressedHandle;
        inputReader.OnToggleKeyPressed += OnToggleKeyPressedHandle;

        IngameManager.Instance.ToggleCurrentSpawner(player, 1);
    }

    public override void Release()
    {
        if(player.IsOwner == false)
            return;
        
        inputReader.OnNumberKeyPressed -= OnNumberKeyPressedHandle;
        inputReader.OnArrowKeyPressed -= OnArrowKeyPressedHandle;
        inputReader.OnToggleKeyPressed -= OnToggleKeyPressedHandle;
    }

    private void OnNumberKeyPressedHandle(int index)
    {
        if(index == 0)
            index = 10;
        index--;

        IngameManager manager = IngameManager.Instance;
        manager.CurrentSpawner?.SpawnUnit(index, manager.FocusedLine);
    }

    private void OnArrowKeyPressedHandle(float value)
    {
        currentLine += (int)Mathf.Sign(value);
        currentLine %= 3;
        if(currentLine < 0)
            currentLine = 2;

        IngameManager.Instance.ToggleCurrentSpawner(player, currentLine);
    }

    private void OnToggleKeyPressedHandle()
    {
        Debug.Log("Toggle Pressed");

    }
}
