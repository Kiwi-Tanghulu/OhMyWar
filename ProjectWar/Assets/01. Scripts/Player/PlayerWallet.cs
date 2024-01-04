using System;
using UnityEngine;

public class PlayerWallet : PlayerComponent
{
	private int increaseAmount = 10;

    private float lastEarnTime = 0f;
    
    public event Action<int, int> OnGoldChanged;

    public override void Init(Player player)
    {
        base.Init(player);

        if(player.IsOwner == false)
            Destroy(this);

        GameObject.Find("Canvas").transform.Find("InGameUI/WalletPanel").GetComponent<WalletPanel>().Init(this);
    }

    private void Update()
    {
        if(Time.time - lastEarnTime < 1)
            return;
        
        player.ModifyGold(increaseAmount);
        OnGoldChanged?.Invoke(player.Gold, player.MaxGold);
        lastEarnTime = Time.time;
    }

    public void ModifyAmount(int amount)
    {
        increaseAmount += amount;
        increaseAmount = Mathf.Max(0, increaseAmount);
    }

    public void ModifyAmountFactor(float factor)
    {
        increaseAmount = (int)(increaseAmount * factor);
        increaseAmount = Mathf.Max(0, increaseAmount);
    }

    public void SetAmount(int amount)
    {
        increaseAmount = amount;
    }

    public void SetMaxGold(int maxGold)
    {
        player.SetMaxGold(maxGold);
    }

    public void ModifyMaxGoldFactor(float factor)
    {
        player.SetMaxGold((int)(player.MaxGold * factor));
    }
}
