using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitContainer : FixedUI
{
    [SerializeField] UnitInfoSO[] unitInfos;
    [SerializeField] private Image[] images;
    [SerializeField] private TextMeshProUGUI[] costTexts;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < unitInfos.Length; i++)
        {
            images[i].sprite = unitInfos[i].image;
            costTexts[i].text = unitInfos[i].cost.ToString();
        }
    }

    [ContextMenu("SettingInfo")]
    private void SettingInfo()
    {
        for (int i = 0; i < unitInfos.Length; i++)
        {
            images[i].sprite = unitInfos[i].image;
            costTexts[i].text = unitInfos[i].cost.ToString();
        }
    }
}
