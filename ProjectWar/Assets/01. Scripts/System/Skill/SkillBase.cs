using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class SkillBase : NetworkBehaviour, IOperable<Player>
{
    [SerializeField] protected int cost = 500;
    
    [Space(10f)]
    [SerializeField] UnityEvent OnSkillActivedEvent;

    public int Cost => cost;

    protected Player player = null;

    protected abstract bool ActiveSkill();

    public bool Operate(Player performer = null)
    {
        player = performer;
        bool result = ActiveSkill();
        if(result)
            OnSkillActivedEvent?.Invoke();

        return result;
    }
}
