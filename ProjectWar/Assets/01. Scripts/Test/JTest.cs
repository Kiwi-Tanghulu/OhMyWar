using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class JTest : NetworkBehaviour
{
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
            UnitManager.Instance.SpawnUnit(UnitType.Infantry, NetworkManager.Singleton.LocalClientId, Vector2.zero, Vector2.zero);
    }
}
