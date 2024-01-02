using UnityEngine;

public class PlayerAttack : PlayerComponent
{
	[SerializeField] InputReader inputReader;

    [Space(10f)]
    [SerializeField] float range = 5;
    [SerializeField] int damage = 10;
    [SerializeField] LayerMask detectLayer;

    private Collider2D[] others = new Collider2D[5];

    public override void Init(Player player)
    {
        base.Init(player);

        if(player.IsOwner)
            inputReader.OnRightClicked += OnRightClickedHandle;
    }

    public override void Release()
    {
        if(player.IsOwner)
            inputReader.OnRightClicked -= OnRightClickedHandle;
    }

    private void OnRightClickedHandle(bool value)
    {
        if(value == false)
            return;

        Vector2 position = inputReader.MousePosition;
        if((position - (Vector2)transform.position).sqrMagnitude > range * range)
            return;

        DetectTargets(position);
        for(int i = 0; i < others.Length; ++i)
        {
            if(others[i] == null)
                return;

            if(others[i].TryGetComponent<IDamageable<Player>>(out IDamageable<Player> id))
                id?.TakeDamage(damage, player.OwnerClientId);
        }
    }

    private void DetectTargets(Vector3 position)
    {
        Physics2D.OverlapCircleNonAlloc(position, range, others, detectLayer);
    }
}
