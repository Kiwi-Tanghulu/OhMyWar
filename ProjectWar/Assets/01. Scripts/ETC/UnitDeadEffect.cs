using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitDeadEffect : NetworkBehaviour
{
    private NetworkObject unit;

    public void SetUnit(UnitController _unit)
    {
        unit = _unit.GetComponent<NetworkObject>();
    }

    public void UnitDead()
    {
        if (GameManager.Instance.HostID.Value == NetworkManager.Singleton.LocalClientId)
        {
            unit.Despawn(true);
        }
    }
}
