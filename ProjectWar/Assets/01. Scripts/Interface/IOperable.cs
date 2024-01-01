using UnityEngine;

// 수동
public interface IOperable
{
    /// <summary>
    /// operate this operable object
    /// </summary>
    /// <returns>operate succeed</returns>
    public bool Operate(GameObject performer = null);
}