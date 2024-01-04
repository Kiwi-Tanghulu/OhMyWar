using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealAttack : UnitAttack
{
    [SerializeField] private float healAmount;
    [SerializeField] private ParticleSystem healEffect;

    private ParticleSystem effect;

    private void Start()
    {
        effect = Instantiate(healEffect, transform);
    }

    public override void Attack()
    {
        if (target == null)
            return;

        if (target.TryGetComponent<UnitHealth>(out UnitHealth attackedObj))
        {
            if (target.layer == 6)
            {
                attackedObj.Heal((int)healAmount);
                if (effect != null)
                    effect.Play();
            }
            else
            {
                attackedObj.TakeDamage((int)attackDamage, OwnerClientId);
                if (hitEffect != null)
                {
                    HitEffect effect = Instantiate(hitEffect);
                    effect.transform.position = target.transform.position;
                    effect.Play();
                }
            }

            PlayEffect();
        }
    }

    protected override void PlayEffect()
    {
        if (selfAttackEffect != null)
        {
            selfAttackEffect.transform.position = transform.position;
            selfAttackEffect.Play();
        }
    }
}
