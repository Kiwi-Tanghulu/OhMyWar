using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSkill : SkillBase
{
    [SerializeField] private float teleportRange;
    [SerializeField] private LayerMask targetLayer;
    protected override bool ActiveSkill()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(player.transform.position, teleportRange, targetLayer);
        //need line number & line position
        foreach(var unit in cols)
        {

        }
        return true;
    }
}
