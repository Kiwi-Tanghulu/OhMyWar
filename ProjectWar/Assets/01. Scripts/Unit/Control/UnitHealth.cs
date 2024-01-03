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
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth.Value;
    public bool IsDie => isDie;

    public event Action OnHeal;
    public event Action OnDie;

    private void Awake()
    {
        currentHealth = new NetworkVariable<float>();
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
        SetHealth(-damage);

        if (currentHealth.Value <= 0) 
        {
            Die();
        }
    }

    public void TakeDamage(int damage = 0, ulong performerID = 0, Vector3 point = default)
    {
        if(IsServer)
            OnDamaged(damage, NetworkManager.Singleton.ConnectedClients[performerID].PlayerObject, point);
    }

    public void Stun()
    {
        controller.ChangeState(UnitStateType.Stun);
    }

    private void MaxHealthValueChange(int value) => maxHealth = value;
}
