using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapImage : MonoBehaviour
{
    [SerializeField] private Sprite redIcon;
    [SerializeField] private Sprite blueIcon;
    [SerializeField] private Color redColor;
    [SerializeField] private Color blueColor;
    private SpriteRenderer render;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();    
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
}
