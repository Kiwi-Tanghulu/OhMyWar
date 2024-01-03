using System;
using UnityEngine;

public class PlayerInput : PlayerComponent
{
    [SerializeField] InputReader inputReader;

    public override void Init(Player player)
    {
        base.Init(player);

        if(player.IsOwner == false)
            return;

        inputReader.OnNumberKeyPressed += OnNumberKeyPressedHandle;
        inputReader.OnToggleKeyPressed += OnToggleKeyPressedHandle;
    }

    public override void Release()
    {
        if(player.IsOwner == false)
            return;
        
        inputReader.OnNumberKeyPressed -= OnNumberKeyPressedHandle;
        inputReader.OnToggleKeyPressed -= OnToggleKeyPressedHandle;
    }

    private void OnNumberKeyPressedHandle(int index)
    {
        if(index == 0)
            index = 10;
        index--;

        Debug.Log("Unit Spawn");
        IngameManager manager = IngameManager.Instance;
        manager?.SpawnUnit(index);
    }

    private void OnToggleKeyPressedHandle()
    {
        Debug.Log("Toggle Pressed");

    }
}
