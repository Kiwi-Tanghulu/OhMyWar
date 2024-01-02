using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealAttack : UnitAttack
{
    [SerializeField] private float healAmount;
    [SerializeField] private ParticleSystem dealEffect;

    public override void Attack()
    {
        if (target.TryGetComponent<UnitHealth>(out UnitHealth attackedObj))
        {
            if (target.layer == 6)
            {
                attackedObj.Heal((int)healAmount);
                PlayEffect();
            }
            else
            {
                attackedObj.TakeDamage((int)attackDamage, OwnerClientId);
                if(dealEffect != null)
                {
                    attackEffect.transform.position = target.transform.position;
                    attackEffect.Play();
                }
            }
        }
    }
}
