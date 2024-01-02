using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndPanel : PanelUI
{
    [SerializeField] private Sprite victoryImage;
    [SerializeField] private Sprite failImage;
    [SerializeField] private float titleAnimationDuration;

    private TextMeshProUGUI goldText;
    private TextMeshProUGUI timeText;
    private Image titleImage;

    protected override void Awake()
    {
        base.Awake();
        titleImage = transform.Find("Title").GetComponent<Image>();
        goldText = transform.Find("GameEndInfo/Gold/Text (TMP)").GetComponent<TextMeshProUGUI>();
        timeText = transform.Find("GameEndInfo/Time/Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameEndInfo(false, 517, 3802);
    }
    public void GameEndInfo(bool victory, int time, int gold)
    {
        if (victory)
            titleImage.sprite = victoryImage;
        else
            titleImage.sprite = failImage;

        timeText.text = $"∞‘¿”Ω√∞£ : {time / 60}∫–{time % 60}√ ";
        goldText.text = $"»πµÊ∞ÒµÂ : {gold}";
        Show();
    }

    public override void ShowAnimation()
    {
        base.ShowAnimation();
        StartCoroutine(TitleAnimation());
    }

    private IEnumerator TitleAnimation() 
    {
        float curTime = 0f;
        while(curTime <= titleAnimationDuration)
        {
            titleImage.fillAmount = curTime / titleAnimationDuration;
            curTime += Time.deltaTime;
            yield return null;
        }
    }
}
