using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MeleeAttack : UnitAttack
{
    public override void Attack()
    {
        if(target.TryGetComponent<IDamageable<NetworkObject>>(out IDamageable<NetworkObject> attackedObj))
        {
            Debug.Log(1);
            attackedObj.TakeDamage((int)attackDamage, OwnerClientId);
            PlayEffect();
        }
    }
}
