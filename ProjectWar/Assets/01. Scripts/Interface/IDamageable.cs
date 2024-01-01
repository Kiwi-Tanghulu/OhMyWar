using UnityEngine;

// 능동
public interface IDamageable
{
    /// <summary>
    /// called when it was damaged
    /// </summary>
    public void OnDamaged(int damage = 0, GameObject performer = null, Vector3 point = default);
}