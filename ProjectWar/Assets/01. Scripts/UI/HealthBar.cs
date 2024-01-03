using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform owner;
    private Transform bar;

    private Vector2 originBarSize;
    private Vector2 originBarPos;

    private void Start()
    {
        originBarPos = bar.localPosition;
        originBarSize = bar.localScale;
        
        bar = transform.Find("Bar");
    }

    public void SetHealthBar(float percent)
    {
        float changeAmount = originBarSize.x - originBarSize.x * percent;

        bar.localScale = new Vector2(originBarSize.x - changeAmount, originBarSize.y);
        bar.localPosition = new Vector2(originBarPos.x - changeAmount * 0.5f, originBarPos.y);
    }
}
