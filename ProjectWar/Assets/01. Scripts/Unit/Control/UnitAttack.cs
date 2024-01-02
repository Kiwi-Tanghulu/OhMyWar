using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttack : UnitComponent
{
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected bool canAttack = false;
    
    private WaitForSeconds wfs;

    public float AttackDamage => attackDamage;
    public float AttackDistance => attackDistance;
    public float AttackDelay => attackDelay;
    public bool CanAttack => canAttack;

    private void Awake()
    {
        wfs = new WaitForSeconds(attackDelay);   
    }

    public void DoAttack()
    {
        if (!IsServer)
            return;

        if (!canAttack)
            return;

        canAttack = false;

        Attack();

        StartCoroutine(Delay());
    }

    protected abstract void Attack();

    protected IEnumerator Delay()
    {
        yield return wfs;

        canAttack = true;
    }
}
