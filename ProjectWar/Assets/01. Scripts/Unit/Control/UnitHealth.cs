using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UnitHealth : UnitComponent, IDamageable<NetworkObject>, IStunable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool isDie;
    private NetworkVariable<float> currentHealth;
    private HealthBar healthBar;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth.Value;
    public bool IsDie => isDie;
    public event Action OnHeal;
    public event Action OnDie;

    public GameObject shieldEffect;

    [SerializeField] private float shieldAmount;

    private void Awake()
    {
        currentHealth = new NetworkVariable<float>();
        healthBar = transform.Find("HealthBar").GetComponent<HealthBar>();
    }

    public override void InitCompo(UnitController _controller)
    {
        base.InitCompo(_controller);
        this.maxHealth = controller.Info.maxHealth;

        if (IsServer)
        {
            currentHealth.Value = maxHealth;
        }

        controller.Stat.GetStat(UnitStatType.maxHealth).OnValueChangeEvent += MaxHealthValueChange;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        controller.Stat.GetStat(UnitStatType.maxHealth).OnValueChangeEvent -= MaxHealthValueChange;
    }

    private void SetHealth(float value)
    {
        if (IsServer)
        {
            currentHealth.Value = Mathf.Clamp(currentHealth.Value + value, 0, maxHealth);
            healthBar.SetHealthBar(currentHealth.Value / maxHealth);
        }
    }

    public void Heal(float value)
    {
        if (!IsServer)
            return;

        SetHealth(value);
        HealClientRpc();
    }
    [ClientRpc] private void HealClientRpc()
    {
        OnHeal?.Invoke();
    }

    public void Die()
    {
        if (!IsServer)
            return;

        controller.ChangeState(UnitStateType.Die);
        isDie = true;
        DieClientRpc();
    }
    [ClientRpc] private void DieClientRpc()
    {
        OnDie?.Invoke();
    }

    public void OnDamaged(int damage = 0, NetworkObject performer = null, Vector3 point = default)
    {
        float dmg = damage;

        if(shieldAmount != 0)
        {
            dmg = damage - shieldAmount;
            shieldAmount -= damage;

            if(shieldAmount <= 0)
            {
                shieldAmount = Mathf.Max(shieldAmount, 0);
                //something
            }
        }

        if (dmg <= 0)
            return;

        SetHealth(-dmg);

        if (currentHealth.Value <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage = 0, ulong performerID = 0, Vector3 point = default)
    {
        if (IsServer)
            OnDamaged(damage, NetworkManager.Singleton.ConnectedClients[performerID].PlayerObject, point);
    }

    public void Stun()
    {
        controller.ChangeState(UnitStateType.Stun);
    }

    private void MaxHealthValueChange(int value) 
    {
        maxHealth = value;
        healthBar.SetHealthBar(currentHealth.Value / maxHealth);
    }

    public void SetShield(float value)
    {
        shieldAmount += value;

        //actove shield effect;
    }
}
