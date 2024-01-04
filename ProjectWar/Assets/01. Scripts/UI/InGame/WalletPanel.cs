using System;
using UnityEngine;

public class WalletPanel : PanelUI
{
    public void Init(PlayerWallet wallet)
    {
        wallet.OnGoldChanged += HandleGoldChanged;
    }

    private void HandleGoldChanged(int gold)
    {
        // gold로 텍스트 띄우기

        // 조건 골드보다 많으면
        if(gold >= 100)
        {
            // UI 띄우기
        }
    }

    public void UpgradeWallet()
    {
        
    }
}
