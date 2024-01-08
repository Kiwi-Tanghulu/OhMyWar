using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SightObject : NetworkBehaviour
{
    [SerializeField] private GameObject minimapPoint;
    [SerializeField] private Sprite redIcon;
    [SerializeField] private Sprite blueIcon;
    [SerializeField] private Color redColor;
    [SerializeField] private Color blueColor;
    private SpriteRenderer render;
    private bool isSight;

    public void Init(TeamType team)
    {
        isSight = false;
        SetSight(false);
        render = minimapPoint.GetComponent<SpriteRenderer>();

        //MinimapManager.Instance.RegistSightObject(this);

        ChangeImage(team);
    }

    public void ChangeImage(TeamType team)
    {
        if (team == TeamType.Blue)
        {
            render.sprite = blueIcon;
            render.color = blueColor;
        }
        else
        {
            render.sprite = redIcon;
            render.color = redColor;
        }
    }

    public void SetSight(bool value)
    {
        isSight = value;
        minimapPoint.SetActive(value);
    }
}
