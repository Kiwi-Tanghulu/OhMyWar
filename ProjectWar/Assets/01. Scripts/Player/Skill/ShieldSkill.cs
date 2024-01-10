using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : SkillBase
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float radius;
    [SerializeField] private float shieldAmount;
    [SerializeField] AnimationEffect effect;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        targetLayer = 1 << (transform.root.gameObject.layer);
    }

    protected override bool ActiveSkill()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll((Vector2)transform.position, radius, targetLayer);

        AnimationEffect _effect = Instantiate(effect, transform.position, Quaternion.identity);
        _effect.Play();

        if(IsServer)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].TryGetComponent<UnitHealth>(out UnitHealth u))
                {
                    u.SetShield(shieldAmount);
                }
            }
        }
        

        return true;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    #endif
}
