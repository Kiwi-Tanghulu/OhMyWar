using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GameInfoPanel : PanelUI
{
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
        gameInfoPanelTrm.DOAnchorPos(showTrm.position, showDuration).SetEase(Ease.OutQuart);
    }

    public override void HideAnimation()
    {
        gameInfoPanelTrm.DOAnchorPos(hideTrm.position, hideDuration).SetEase(Ease.InOutQuint);
    }

    private void HandleGameInfoPanel()
    {
        if (IsOpen)
            Hide();
        else
            Show();
    }
}
