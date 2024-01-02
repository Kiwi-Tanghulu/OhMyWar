using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UnitHealth : UnitComponent, IDamageable<NetworkObject>
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool isDie;
    private NetworkVariable<float> currentHealth;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth.Value;
    public bool IsDie => isDie;

    public event Action OnHeal;
    public event Action OnDie;

    public override void InitCompo(UnitController _controller)
    {
        base.InitCompo(_controller);
        currentHealth = new NetworkVariable<float>();
        this.maxHealth = controller.Info.maxHealth;
    }

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
        if (!IsServer)
            return;

        SetHealth(damage);

        if (currentHealth.Value <= 0) 
        {
            Die();
        }
    }

    public void TakeDamage(int damage = 0, ulong performerID = 0, Vector3 point = default)
    {
        OnDamaged(damage, NetworkManager.Singleton.ConnectedClients[performerID].PlayerObject, point);
    }
}
