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
        Vector3 dir = Vector3.right * player.GetComponent<PlayerMovement>().FacingDirection;
        Quaternion rotation = Quaternion.Euler(0, ((dir.x == -1) ? 180f : 0f), 0);

        if(player.IsHost)
        {
            instance = Instantiate(serverProjectile, firePosition.position, rotation);
            instance.GetComponent<DealDamageOnContact>()?.Init(player);
        }
        else
            instance = Instantiate(clientProjectile, firePosition.position, rotation);

        instance.GetComponent<Rigidbody2D>().velocity = dir * projectileSpeed;

        return true;
    }
}
