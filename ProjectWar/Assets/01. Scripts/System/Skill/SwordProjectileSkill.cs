using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectileSkill : SkillBase
{
    [SerializeField] private GameObject clientProjectile;
    [SerializeField] private GameObject serverProjectile;
    protected override bool ActiveSkill()
    {
        return true;
    }
}
