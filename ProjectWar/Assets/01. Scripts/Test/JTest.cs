using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class JTest : NetworkBehaviour
{
    public NetworkObject unit;

    // Start is called before the first frame update
    [ServerRpc]
    public void SpawnServerRpc()
    {
        NetworkObject u = Instantiate(unit);
        u.Spawn();
        u.transform.position = Vector3.zero + Vector3.forward;
        u.GetComponent<UnitMovement>().SetTargetPosition(Vector2.right * 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
