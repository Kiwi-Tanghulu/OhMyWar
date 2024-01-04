using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeSkill : SkillBase
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float radius;
    [SerializeField] private Vector2 offset;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        targetLayer = TeamManager.Instance.GetEnemyLayer(transform.root.gameObject.layer);
    }

    protected override bool ActiveSkill()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll((Vector2)transform.position + offset, radius, targetLayer);

        for(int i = 0; i < cols.Length; i++)
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
