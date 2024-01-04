using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnitAttack : UnitComponent
{
    protected float attackDamage;
    protected float attackDistance;
    protected float attackDelay;
    protected float serchDelay;
    protected bool canAttack = true;
    protected bool shouldAttack = false;
    protected LayerMask targetLayer;
    protected GameObject target;
    //protected ParticleSystem attackEffect;
    protected ParticleSystem selfAttackEffect;
    protected string soundName;
    public HitEffect hitEffect;

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
        //if(controller.Info.attackEffect != null)
        //    this.attackEffect = Instantiate(controller.Info.attackEffect, transform);
        if (controller.Info.selfAttackEffect != null)
            this.selfAttackEffect = Instantiate(controller.Info.selfAttackEffect, transform);

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

    protected virtual void PlayEffect()
    {
        //if (attackEffect != null)
        //{
        //    attackEffect.transform.position = target.transform.position;
        //    attackEffect.Play();
        //}

        if(hitEffect != null)
        {
            HitEffect effect = Instantiate(hitEffect);
            effect.transform.position = target.transform.position;
            effect.Play();
        }

        if (selfAttackEffect != null)
        {
            selfAttackEffect.transform.position = transform.position;
            selfAttackEffect.Play();
        }
    }


    private bool SerchTarget()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, AttackDistance, targetLayer);

        if(col != null && col.CompareTag("Player") == false)
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

    private void AttackDamageValueChange(float value) => attackDamage = value;
    private void AttackDistanceValueChange(float value) => attackDistance = value;
    private void AttackDelayValueChange(float value)
    {
        attackDelay = value;
        attackWfs = new WaitForSeconds(attackDelay);
    }
}