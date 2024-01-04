using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnitAttack : UnitComponent
{
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float serchDelay;
    [SerializeField] protected bool canAttack = true;
    [SerializeField] protected bool shouldAttack = false;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected GameObject target;
    [SerializeField] protected ParticleSystem attackEffect;
    [SerializeField] protected string soundName;

    private WaitForSeconds attackWfs;
    private WaitForSeconds serchWfs;

    public bool ShouldAttack => shouldAttack;

    public float AttackDamage => attackDamage;
    public float AttackDistance => attackDistance;
    public float AttackDelay => attackDelay;
    public bool CanAttack => canAttack;

    public UnityEvent OnAttackStartEvetn;

    public override void InitCompo(UnitController _controller)
    {
        base.InitCompo(_controller);

        this.attackDamage = controller.Info.attackDamage;
        this.attackDistance = controller.Info.attackDistance;
        this.attackDelay = controller.Info.attackDelay;
        this.serchDelay = controller.Info.serchDelay;
        this.targetLayer = controller.Info.targetLayer ^ (1 << gameObject.layer);
        this.attackEffect = Instantiate(controller.Info.attackEffect, transform);

        attackWfs = new WaitForSeconds(attackDelay);
        serchWfs = new WaitForSeconds(serchDelay);
        canAttack = true;

        if (IsServer)
            StartCoroutine(SerchDelayCo());

        controller.Stat.GetStat(UnitStatType.attackDamage).OnValueChangeEvent += AttackDamageValueChange;
        controller.Stat.GetStat(UnitStatType.attackDistance).OnValueChangeEvent += AttackDistanceValueChange;
        controller.Stat.GetStat(UnitStatType.attackDelay).OnValueChangeEvent += AttackDelayValueChange;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        controller.Stat.GetStat(UnitStatType.attackDamage).OnValueChangeEvent -= AttackDamageValueChange;
        controller.Stat.GetStat(UnitStatType.attackDistance).OnValueChangeEvent -= AttackDistanceValueChange;
        controller.Stat.GetStat(UnitStatType.attackDelay).OnValueChangeEvent -= AttackDelayValueChange;
    }

    public bool StartAttack()
    {
        if (!IsServer)
            return false;
        
        if (!canAttack)
            return false;
        
        canAttack = false;

        StartAttackClientRpc();
        StartCoroutine(AttackDelayCo());

        return true;
    }

    [ClientRpc]
    private void StartAttackClientRpc()
    {
        OnAttackStartEvetn?.Invoke();
    }

    public abstract void Attack();

    protected void PlayEffect()
    {
        if (attackEffect == null)
            return;

        attackEffect.transform.position = target.transform.position;
        attackEffect.Play();
    }


    private bool SerchTarget()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, AttackDistance, targetLayer);

        if(col != null)
        {
            target = col.gameObject;
            shouldAttack = true;
            return true;
        }

        return false;
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

            if(target == null)
            {
                shouldAttack = SerchTarget();
            }
            else
            {
                if(target.layer == gameObject.layer)
                    shouldAttack = SerchTarget();
            }
        }
    }

    private void AttackDamageValueChange(int value) => attackDamage = value;
    private void AttackDistanceValueChange(int value) => attackDistance = value;
    private void AttackDelayValueChange(int value)
    {
        attackDelay = value;
        attackWfs = new WaitForSeconds(attackDelay);
    }
}