using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSkill : SkillBase
{
    [SerializeField] private float teleportRange;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float teleportDelay;

    [SerializeField] private GameObject teleportEffect;
    protected override bool ActiveSkill()
    {
        if (player.IsHost)
        {

        }
        
        return true;
    }
}
