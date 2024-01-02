using UnityEngine;

public class PlayerWallet : PlayerComponent
{
	private int increaseAmount = 1;

    private float lastEarnTime = 0f;

    public override void Init(Player player)
    {
        base.Init(player);

        if(player.IsOwner == false)
            Destroy(this);
    }

    private void Update()
    {
        if(Time.time - lastEarnTime < 1)
            return;
        
        player.ModifyGold(increaseAmount);
        lastEarnTime = Time.time;
    }

    public void ModifyAmount(int amount)
    {
        increaseAmount += amount;
        increaseAmount = Mathf.Max(0, increaseAmount);
    }
}
