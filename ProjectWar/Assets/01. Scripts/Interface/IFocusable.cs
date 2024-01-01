using UnityEngine;

// 능동
public interface IFocusable
{
    /// <summary>
    /// called when it was focused
    /// </summary>
    /// <returns>focus succeed</returns>
    public bool OnFocused(GameObject performer = null, Vector3 point = default);
}