using UnityEngine;

// 수동
public interface IOperable<T> where T : class
{
    /// <summary>
    /// operate this operable object
    /// </summary>
    /// <returns>operate succeed</returns>
    public bool Operate(T performer = null);
}