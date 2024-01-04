using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : SkillBase
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float radius;
    [SerializeField] HitEffect effect;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        targetLayer = 1 << (transform.root.gameObject.layer - 1);
    }

    protected override bool ActiveSkill()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll((Vector2)transform.position, radius, targetLayer);

        HitEffect _effect = Instantiate(effect, transform.position, Quaternion.identity);
        _effect.Play();

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].TryGetComponent<IStunable>(out IStunable stunObj))
            {
                stunObj.Stun();
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
