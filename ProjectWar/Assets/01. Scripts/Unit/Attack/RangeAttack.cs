using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RangeAttack : UnitAttack
{
    [SerializeField] NetworkObject projectile;

    public override void Attack()
    {
        NetworkObject pro = Instantiate(projectile, transform.position, Quaternion.identity);
        pro.Spawn();
    }
}
