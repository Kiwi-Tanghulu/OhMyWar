using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RangeAttack : UnitAttack
{
    private WaitForSeconds wfs;
    private Projectile pro;

    public override void InitCompo(UnitController _controller)
    {
        base.InitCompo(_controller);

        wfs = new WaitForSeconds(controller.Info.takeDamageDelay);
    }

    public override void Attack()
    {
        if (target == null)
            return;

        if (target.TryGetComponent<IDamageable<NetworkObject>>(out IDamageable<NetworkObject> attackedObj))
        {
            StartCoroutine(Delay(attackedObj));
            CreateProjectileServerRpc();
        }
    }

    [ServerRpc]
    private void CreateProjectileServerRpc()
    {
        pro = Instantiate(controller.Info.projectile, transform.position, Quaternion.identity);
        pro.GetComponent<NetworkObject>().Spawn();
        pro.Init(target, (target.transform.position - transform.position).normalized);
    }

    private IEnumerator Delay(IDamageable<NetworkObject> attackedObj)
    {
        yield return wfs;

        attackedObj.TakeDamage((int)attackDamage, OwnerClientId);
        PlayEffect();
        pro.GetComponent<NetworkObject>().Despawn();
    }
}
