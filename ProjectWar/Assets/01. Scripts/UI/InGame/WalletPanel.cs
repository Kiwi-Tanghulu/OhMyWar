using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletPanel : PanelUI
{
    [SerializeField] TMP_Text goldText;
    [SerializeField] Button upgradeButton;

    [Space(10f)]
    [SerializeField] List<Vector3> factors;

    private int level = 1;

    private PlayerWallet wallet;

    public void Init(PlayerWallet wallet)
    {
        this.wallet= wallet;
        wallet.OnGoldChanged += HandleGoldChanged;
    }

    private void HandleGoldChanged(int gold, int maxGold)
    {
        // gold로 텍스트 띄우기
        goldText.text = $"{gold}/{maxGold}";

        // 조건 골드보다 많으면 UI 띄우기
        bool levelCondition = level < factors.Count;
        bool moneyCondition = gold >= factors[level - 1].z;
        upgradeButton.interactable = levelCondition & moneyCondition;
    }

    public void UpgradeWallet()
    {
        level++;
        level = Mathf.Min(level, factors.Count);

        // 계수는 수정하자
        wallet.ModifyAmountFactor(factors[level - 1].x);
        wallet.ModifyMaxGoldFactor(factors[level - 1].y);
    }
}
