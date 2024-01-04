using System;
using UnityEngine;

public class PlayerWallet : PlayerComponent
{
	private int increaseAmount = 1;

    private float lastEarnTime = 0f;
    
    public event Action<int> OnGoldChanged;

    public override void Init(Player player)
    {
        base.Init(player);

        if(player.IsOwner == false)
            Destroy(this);

        GameObject.Find("MainCanvas").transform.Find("WalletPanel").GetComponent<WalletPanel>().Init(this);
    }

    private void Update()
    {
        if(Time.time - lastEarnTime < 1)
            return;
        
        player.ModifyGold(increaseAmount);
        OnGoldChanged?.Invoke(player.Gold);
        lastEarnTime = Time.time;
    }

    public void ModifyAmount(int amount)
    {
        increaseAmount += amount;
        increaseAmount = Mathf.Max(0, increaseAmount);
    }

    public void ModifyFactor(float factor)
    {
        increaseAmount = (int)(increaseAmount * factor);
        increaseAmount = Mathf.Max(0, increaseAmount);
    }
}
