using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GameInfoPanel : PanelUI
{
    [SerializeField] private GameObject knightPanel;
    [SerializeField] private GameObject magicPanel;

    [SerializeField] private InputReader inputReader;

    [SerializeField] private RectTransform showTrm;
    [SerializeField] private RectTransform hideTrm;

    [SerializeField] private float showDuration;
    [SerializeField] private float hideDuration;
    private RectTransform gameInfoPanelTrm;

    protected override void Awake()
    {
        base.Awake();
        gameInfoPanelTrm = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        inputReader.OnToggleKeyPressed += HandleGameInfoPanel;
    }
    private void OnDisable()
    {
        inputReader.OnToggleKeyPressed -= HandleGameInfoPanel;
    }
    public override void ShowAnimation()
    {
        gameInfoPanelTrm.DOAnchorPos(showTrm.anchoredPosition, showDuration).SetEase(Ease.OutQuart);
        Debug.Log("show");
    }

    public override void HideAnimation()
    {
        gameInfoPanelTrm.DOAnchorPos(hideTrm.anchoredPosition, hideDuration).SetEase(Ease.InOutQuint);
        Debug.Log("hide");
    }

    private void HandleGameInfoPanel()
    {
        if (IsOpen)
            Hide();
        else
            Show();
    }

    public void SettingSkillIcon(CharacterType characterType)
    {
        if (characterType == CharacterType.Knight)
        {
            knightPanel.SetActive(true);
        }
        else
        {
            magicPanel.SetActive(true);
        }
    }
}
