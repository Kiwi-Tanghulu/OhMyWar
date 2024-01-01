using UnityEngine;

public abstract class PlayerComponent : MonoBehaviour
{
    protected Player player = null;

    public virtual void Init(Player player)
    {
        this.player = player;
    }

    public virtual void Release() {}
}
