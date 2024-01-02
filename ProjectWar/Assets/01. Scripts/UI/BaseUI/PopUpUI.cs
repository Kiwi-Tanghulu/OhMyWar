using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUI : BaseUI
{
    protected override void Awake()
    {
        Hide();
    }

    public virtual void Show(float time, Transform parent = null)
    {
        Show();
        StartCoroutine(Hide(time));
    }

    private IEnumerator Hide(float time)
    {
        yield return new WaitForSeconds(time);

        Hide();
    }
}
