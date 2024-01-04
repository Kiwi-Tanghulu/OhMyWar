using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitDeadEffect : MonoBehaviour
{
    public void UnitDead()
    {
        transform.root.GetComponent<NetworkObject>().Despawn();
    }
}
