using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class StructureBase : NetworkBehaviour, IDamageable<NetworkClient>
{
    [SerializeField] protected int maxHP = 100;
    private int currentHP = 0;
    public int HP => currentHP;

    [SerializeField] protected UnityEvent<NetworkClient> OnDestroyedEvent;
    [SerializeField] protected UnityEvent<NetworkClient, Vector3, int> OnDamagedEvent;

    private bool isDestroyed = false;

    // 실질적으로 데미지를 넣는 함수
    public virtual void OnDamaged(int damage = 0, NetworkClient performer = null, Vector3 point = default)
    {
        if(isDestroyed)
            return;

        currentHP -= damage;
        OnDamagedEvent?.Invoke(performer, point, damage);

        if(currentHP <= 0)
        {
            isDestroyed = true;
            OnDestroyedEvent?.Invoke(performer);
        }
    }

    // 데미지를 넣기 위해 호출하는 함수
    public void TakeDamage(int damage = 0, ulong performerID = 0, Vector3 point = default)
    {
        // 호스트가 때렸으면 그냥 때리기
        // 게스트가 때렸으면 호스트한테 때렸다고 알리기
        // 그럼 호스트는 때리는 거 적용 시키고 클라한테 때리라고 말하기
        if(performerID == 0)
            return;

        TakeDamageServerRPC(damage, performerID, point);
    }

    [ServerRpc] // 호스트가 모든 클라의 온대미지 호출
    private void TakeDamageServerRPC(int damage = 0, ulong performerID = 0, Vector3 point = default)
    {
        TakeDamageClientRPC(damage, performerID, point);
    }
    
    [ClientRpc] // 실질적 대미지 입히기
    private void TakeDamageClientRPC(int damage = 0, ulong performerID = 0, Vector3 point = default)
    {
        OnDamaged(damage, NetworkManager.Singleton.ConnectedClients[performerID], point);
    }

    public void ModifyHP(int value)
    {
        currentHP += value;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }
}
