using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TeamManager : NetworkBehaviour
{
    public static TeamManager Instance;

    [field: SerializeField]
    public LayerMask RedLayer { get; private set; }
    [field: SerializeField]
    public LayerMask BlueLayer { get; private set; }
    [field: SerializeField]
    public LayerMask NeutralityLayer { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ChangeTeam(GameObject changeedObj, GameObject changingObj)
    {
        changeedObj.layer = changingObj.layer;
    }

    public LayerMask GetEnemyLayer(int layer)
    {
        LayerMask myLayer = 1 << layer;

        return myLayer == BlueLayer ? RedLayer : BlueLayer;
    }

    public bool IsFriendly(GameObject obj)
    {
        if (IsServer)
            return 1 << obj.layer == BlueLayer.value;
        else
            return 1 << obj.layer == RedLayer.value;
    }
}
