using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    private int gold = 0;
    public int Gold => gold;

    private List<PlayerComponent> components;

    private void Start()
    {
        if(IsHost && IsOwner)
            IngameManager.Instance.RegisterPlayer(this, true);
        else
            IngameManager.Instance.RegisterPlayer(this, false);
    }

    public override void OnNetworkSpawn()
    {
        components = new List<PlayerComponent>();
        GetComponents<PlayerComponent>(components);
        components.ForEach(component => component?.Init(this));

        if (IsOwner)
            IngameManager.Instance.OwnerPlayer = transform;
            //CameraManager.Instance.MainVCam.Follow = transform;
    }

    private void Update()
    {
        if(IsOwner == false)
            return;

        if (Keyboard.current.qKey.isPressed)
            ModifyGold(1000);
    }

    public override void OnNetworkDespawn()
    {
        components.ForEach(component => component?.Release());
    }

    public void ModifyGold(int value)
    {
        gold += value;
        gold = Mathf.Max(0, gold);
    }
}
