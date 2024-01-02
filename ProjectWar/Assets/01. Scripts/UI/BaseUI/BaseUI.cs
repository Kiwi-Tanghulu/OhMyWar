using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    public UIType uiType;

    [field: SerializeField] public bool IsOpen { get; protected set; } = true;
    [field: SerializeField] public bool IsWorld { get; protected set; } = true;
    [field: SerializeField] public bool IsPooling { get; protected set; } = true;

    protected virtual void Awake()
    {
        if (!IsOpen)
            HideAnimation();
    }

    public virtual void Show()
    {
        if (IsOpen)
            return;

        ShowAnimation();

        IsOpen = true;
    }

    public virtual void Hide()
    {
        if (!IsOpen)
            return;

        HideAnimation();

        IsOpen = false;
    }

    public virtual void ShowAnimation()
    {
        transform.localScale = Vector3.one;
    }

    public virtual void HideAnimation()
    {
        transform.localScale = Vector3.zero;
    }
}
