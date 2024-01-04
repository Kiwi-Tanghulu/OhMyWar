using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PassiveSkill : NetworkBehaviour
{
    public UnitStatType effectStatType;
    public int chageAmount;

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        ActivePassiveSkill(col.gameObject);   
    }

    protected virtual void OnTriggerExit2D(Collider2D col)
    {
        DeactivePassiveSkill(col.gameObject);
    }

    private void ActivePassiveSkill(GameObject obj)
    {
        if (obj.TryGetComponent<UnitStat>(out UnitStat stat))
        {
            stat.AddModifier(effectStatType, chageAmount);
        }
    }

    private void DeactivePassiveSkill(GameObject obj)
    {
        if (obj.TryGetComponent<UnitStat>(out UnitStat stat))
        {
            stat.RemoveModifier(effectStatType, chageAmount);
        }
    }
}
