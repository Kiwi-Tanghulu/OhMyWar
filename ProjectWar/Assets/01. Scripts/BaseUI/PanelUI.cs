using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelUI : BaseUI
{
    public override void Show()
    {
        base.Show();
        UIManager.Instance.PushPanelUI(this);
    }

    public override void Hide()
    {
        base.Hide();
        UIManager.Instance.PopPanelUI();
    }
}
