using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RangeAttack : UnitAttack
{
    [SerializeField] NetworkObject projectile;

    public override void InitCompo(UnitController _controller)
    {
        base.InitCompo(_controller);

        this.projectile = controller.Info.projectile;
    }

    public override void Attack()
    {
        NetworkObject pro = Instantiate(projectile, transform.position, Quaternion.identity);
        pro.Spawn();
    }
}
