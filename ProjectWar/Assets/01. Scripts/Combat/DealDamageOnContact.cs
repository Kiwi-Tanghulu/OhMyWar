using UnityEngine;
using Unity.Netcode;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject effect;

    private Player performer = null;

    public void Init(Player performer)
    {
        this.performer = performer;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string target = performer.IsBlue ? "RedUnit" : "BlueUnit";
        if(other.CompareTag(target) == false)
            return;

        if(other.TryGetComponent<IDamageable<NetworkObject>>(out IDamageable<NetworkObject> id))
            id?.TakeDamage(damage, performer.OwnerClientId);
    }
}
