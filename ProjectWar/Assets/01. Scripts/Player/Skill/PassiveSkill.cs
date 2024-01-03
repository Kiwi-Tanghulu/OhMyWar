using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PassiveSkill : NetworkBehaviour
{
    public UnitStatType effectStatType;
    public int chageAmount;

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
