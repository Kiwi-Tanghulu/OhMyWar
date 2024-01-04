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
        // GameEndInfo(false, 517, 3802);
        IngameManager.Instance.OnGameFinishedEvent += GameEndInfo;
    }

    public void GameEndInfo(bool victory, int gold, float time)
    {
        if (victory)
            titleImage.sprite = victoryImage;
        else
            titleImage.sprite = failImage;

        timeText.text = $"게임 시간 : {System.TimeSpan.FromSeconds(time).ToString("mm':'ss")}";
        goldText.text = $"획득 골드 : {gold}";
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

    public void CloseGame()
    {
        HostManager.Instance.CloseHost();
        SceneLoader.Instance.LoadSceneAsync("MenuScene");
    }
}
