using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class StructureBase : NetworkBehaviour, IDamageable<NetworkObject>
{
    [SerializeField] protected int maxHP = 100;
    private NetworkVariable<int> currentHP;
    public int HP => currentHP.Value;

    [SerializeField] protected UnityEvent<NetworkObject> OnDestroyedEvent;
    [SerializeField] protected UnityEvent<NetworkObject, Vector3, int> OnDamagedEvent;

    protected bool isDestroyed = false;

    private HealthBar healthBar;

    protected virtual void Awake()
    {
        currentHP = new NetworkVariable<int>(maxHP);
        healthBar = transform.Find("HealthBar").GetComponent<HealthBar>();
    }

    protected virtual void Start()
    {
        if(TeamManager.Instance.IsFriendly(gameObject))
        {
            MinimapManager.Instance.RegistViewObject(GetComponent<ViewObject>());
        }
    }

    // 실질적으로 데미지를 넣는 함수
    public virtual void OnDamaged(int damage = 0, NetworkObject performer = null, Vector3 point = default)
    {
        if (isDestroyed)
            return;

        currentHP.Value -= damage;
        healthBar?.SetHealthBar(currentHP.Value / maxHP);

        if(currentHP.Value <= 0)
        {
            isDestroyed = true;
            OnDie(performer);
            TeamManager.Instance.ChangeTeam(gameObject, performer.gameObject);
        }
    }
    
    public virtual void OnDie(NetworkObject performer)
    {
        OnDestroyedEvent?.Invoke(performer);
        TeamManager.Instance.ChangeTeam(gameObject, performer.gameObject);
        if (TeamManager.Instance.IsFriendly(gameObject))
        {
            MinimapManager.Instance.RegistViewObject(GetComponent<ViewObject>());
        }
    }

    // 데미지를 넣기 위해 호출하는 함수
    public void TakeDamage(int damage = 0, ulong performerID = ulong.MaxValue, Vector3 point = default)
    {
        // 호스트가 때렸으면 그냥 때리기
        // 게스트가 때렸으면 호스트한테 때렸다고 알리기
        // 그럼 호스트는 때리는 거 적용 시키고 클라한테 때리라고 말하기
        
        if (performerID == ulong.MaxValue)
            return;
        
        TakeDamageServerRPC(damage, performerID, point);
    }

    [ServerRpc] // 호스트가 모든 클라의 온대미지 호출
    private void TakeDamageServerRPC(int damage = 0, ulong performerID = 0, Vector3 point = default)
    {
        OnDamaged(damage, NetworkManager.Singleton.ConnectedClients[performerID].PlayerObject, point);
        TakeDamageClientRPC(damage, performerID, point);
    }
    
    [ClientRpc] // 실질적 대미지 입히기
    private void TakeDamageClientRPC(int damage = 0, ulong performerID = 0, Vector3 point = default)
    {
        //OnDamaged(damage, NetworkManager.Singleton.ConnectedClients[performerID].PlayerObject, point);
        OnDamagedEvent?.Invoke(null, point, damage);
        
    }

    public void ModifyHP(int value)
    {
        currentHP.Value += value;
        currentHP.Value = Mathf.Clamp(currentHP.Value, 0, maxHP);
        healthBar?.SetHealthBar(currentHP.Value / maxHP);
    }

    protected IEnumerator DelayCoroutine(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
