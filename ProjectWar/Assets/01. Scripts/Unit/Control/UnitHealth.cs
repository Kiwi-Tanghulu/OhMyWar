using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UnitHealth : UnitComponent, IDamageable<NetworkBehaviour>
{
    [SerializeField] private float maxHealth;
    private NetworkVariable<float> currentHealth;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth.Value;

    public event Action OnHeal;
    public event Action OnDie;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(IsServer)
        {
            currentHealth.Value = maxHealth;
        }
    }

    private void SetHealth(float value)
    {
        if (IsServer)
            currentHealth.Value += value;
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

        DieClientRpc();
    }
    [ClientRpc] private void DieClientRpc()
    {
        OnDie?.Invoke();
    }

    public void OnDamaged(int damage = 0, NetworkBehaviour performer = null, Vector3 point = default)
    {
        if (!IsServer)
            return;

        SetHealth(damage);

        if (currentHealth.Value <= 0) 
        {
            Die();
        }
    }
}
