using UnityEngine;

// 수동
public interface IInteractable
{
    /// <summary>
    /// interact this interactable object
    /// </summary>
    /// <returns>interact succeed</returns>
    public bool Interact(GameObject performer = null);
}
