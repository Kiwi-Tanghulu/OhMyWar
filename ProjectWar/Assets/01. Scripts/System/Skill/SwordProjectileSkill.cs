using UnityEngine;

public class SwordProjectileSkill : SkillBase
{
    [SerializeField] GameObject clientProjectile;
    [SerializeField] GameObject serverProjectile;

    [SerializeField] Transform firePosition;
    [SerializeField] float projectileSpeed = 3f;
    
    protected override bool ActiveSkill()
    {
        GameObject instance = null;
        Vector3 position = firePosition.position;

        int facingDirection = player.GetComponent<PlayerMovement>().FacingDirection;
        Vector3 dir = Vector3.right * facingDirection;
        Quaternion rotation = Quaternion.Euler(0, ((facingDirection == -1) ? 180f : 0f), 0);

        if(player.IsHost)
        {
            instance = Instantiate(serverProjectile, position, rotation);
            instance.GetComponent<DealDamageOnContact>()?.Init(player);
        }
        else
            instance = Instantiate(clientProjectile, position, rotation);

        instance.GetComponent<Rigidbody2D>().velocity = dir * projectileSpeed;

        return true;
    }
}
