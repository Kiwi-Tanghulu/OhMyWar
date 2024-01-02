using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject effect;

    private ulong ownerClientId;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.attachedRigidbody == null) { return; }

        if(collision.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            if(ownerClientId == netObj.OwnerClientId) { return; }
        }
    }
}
