using UnityEngine;

// 능동
public interface IDamageable<T> where T : class
{
    /// <summary>
    /// called when it was damaged
    /// </summary>
    public void OnDamaged(int damage = 0, T performer = null, Vector3 point = default);
    public void TakeDamage(int damage = 0, ulong performerID = 0, Vector3 point = default);
}