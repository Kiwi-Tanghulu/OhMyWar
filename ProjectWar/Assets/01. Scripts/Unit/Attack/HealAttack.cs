using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealAttack : UnitAttack
{
    [SerializeField] private float healAmount;

    public override void Attack()
    {
        if (target.TryGetComponent<UnitHealth>(out UnitHealth attackedObj))
        {
            if (target.CompareTag("Friendly"))
            {
                attackedObj.Heal((int)healAmount);
            }
            else
            {
                attackedObj.TakeDamage((int)attackDamage, OwnerClientId);
            }
        }
    }
}
