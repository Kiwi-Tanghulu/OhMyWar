using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttack : UnitComponent
{
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float serchDelay;
    [SerializeField] protected bool canAttack = false;
    [SerializeField] protected bool shouldAttack = false;
    [SerializeField] protected LayerMask layer;
    [SerializeField] protected GameObject target;

    private WaitForSeconds attackWfs;
    private WaitForSeconds serchWfs;

    public bool ShouldAttack => shouldAttack;

    public float AttackDamage => attackDamage;
    public float AttackDistance => attackDistance;
    public float AttackDelay => attackDelay;
    public bool CanAttack => canAttack;

    private void Awake()
    {
        attackWfs = new WaitForSeconds(attackDelay);
        serchWfs = new WaitForSeconds(serchDelay);   
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(IsServer)
            StartCoroutine(SerchDelayCo());
    }

    public bool StartAttack()
    {
        if (!IsServer)
            return false;

        if (!canAttack)
            return false;

        canAttack = false;

        StartCoroutine(AttackDelayCo());

        return true;
    }

    public abstract void Attack();

    private void SerchTarget()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, AttackDistance, layer, -1, 1);

        if(cols.Length > 0)
        {
            target = cols[0].gameObject;
            shouldAttack = true;
        }
    }

    protected IEnumerator AttackDelayCo()
    {
        yield return attackWfs;

        canAttack = true;
    }

    protected IEnumerator SerchDelayCo()
    {
        while(true)
        {
            yield return serchWfs;

            if (!shouldAttack)
                SerchTarget();
        }
    }
}
