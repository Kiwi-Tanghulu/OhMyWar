using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
public class LobbyPanel : FixedUI
{
    [SerializeField] private TextMeshProUGUI roomCodeText;

    [SerializeField] private Color btnReadyColor;
    [SerializeField] private Color btnUnReadyColor;

    [SerializeField] private Color btnSelectedColor;
    [SerializeField] private RenderTexture[] characterTexture;
    private UserType myType;
    private Color btnDefualtColor = Color.white;

    [SerializeField] private RawImage blueUnitTexture;
    [SerializeField] private Button[] blueCharacterBtn;
    [SerializeField] private GameObject[] blueCharacterSkillIcon;
    private Button blueLastClickCharacterBtn;
    private GameObject blueLastSkillIcon;

    [SerializeField] private RawImage redUnitTexture;
    [SerializeField] private Button[] redCharacterBtn;
    [SerializeField] private GameObject[] redCharacterSkillIcon;
    private Button redLastClickCharacterBtn;
    private GameObject redLastSkillIcon;

    [SerializeField] private RectTransform blueReadyTextTrm;
    [SerializeField] private RectTransform redReadyTextTrm;
    [SerializeField] private Button blueReadyBtn;
    [SerializeField] private Button redReadyBtn;
    [SerializeField] GameObject guideText;

    private LobbyManager lobbyManager;
    public void Init(UserType user, LobbyManager _lobbyManager)
    {
        myType = user;

        lobbyManager = _lobbyManager;

        if (user == UserType.Red)
        {
            transform.Find("RedDefencePanel").gameObject.SetActive(false);
            roomCodeText.gameObject.SetActive(false);
        }
        else
        {
            roomCodeText.text = HostManager.Instance.JoinCode;
        }

        transform.Find($"Blue/KnightSelectBtn").GetComponent<Button>().onClick.AddListener(
            () => lobbyManager.CharacterButtonPressServerRPC(UserType.Blue, CharacterType.Knight));
        transform.Find($"Blue/PsychicSelectBtn").GetComponent<Button>().onClick.AddListener(
            () => lobbyManager.CharacterButtonPressServerRPC(UserType.Blue, CharacterType.Psychic));

        transform.Find($"Red/KnightSelectBtn").GetComponent<Button>().onClick.AddListener(
            () => lobbyManager.CharacterButtonPressServerRPC(UserType.Red, CharacterType.Knight));
        transform.Find($"Red/PsychicSelectBtn").GetComponent<Button>().onClick.AddListener(
            () => lobbyManager.CharacterButtonPressServerRPC(UserType.Red, CharacterType.Psychic));

        SettingPaenlInfo(myType, CharacterType.Knight);
        blueLastClickCharacterBtn = blueCharacterBtn[0];
        redLastClickCharacterBtn = redCharacterBtn[0];

        if (user == UserType.Red)
            HideGuideText();
    }

    public void OnBlueReady()
    {
        lobbyManager.BlueReadyServerRPC();
    }

    public void OnRedReady()
    {
        lobbyManager.RedReadyServerRPC();
    }

    public void ChangeBlueUI()
    {
        blueReadyBtn.image.color = btnReadyColor;
        blueReadyTextTrm.gameObject.SetActive(true);
        transform.Find("BlueDefencePanel").gameObject.SetActive(true);
    }

    public void ChangeRedUI()
    {
        redReadyBtn.image.color = btnReadyColor;
        redReadyTextTrm.gameObject.SetActive(true);
        transform.Find("RedDefencePanel").gameObject.SetActive(true);
    }
    public void ShowClientPanel(ulong id)
    {
        transform.Find("Red").gameObject.SetActive(true);
        transform.Find("BlueDefencePanel").gameObject.SetActive(false);
        HideGuideText();
    }

    public void HideClientPanel(ulong id)
    {
        transform.Find("Red").gameObject.SetActive(false);
        transform.Find("BlueDefencePanel").gameObject.SetActive(true);
    }

    public void SettingPaenlInfo(UserType user, CharacterType character)
    {
        if (user == UserType.Blue)
        {
            if (blueLastSkillIcon != null)
                blueLastSkillIcon.SetActive(false);
            blueUnitTexture.texture = characterTexture[(int)character];

            if (blueLastClickCharacterBtn != null)
                blueLastClickCharacterBtn.image.color = btnDefualtColor;
            blueCharacterBtn[(int)character].image.color = btnSelectedColor;
            blueLastClickCharacterBtn = blueCharacterBtn[(int)character];

            blueCharacterSkillIcon[(int)character].SetActive(true);
            blueLastSkillIcon = blueCharacterSkillIcon[(int)character];
        }
        else
        {
            if (redLastSkillIcon != null)
                redLastSkillIcon.SetActive(false);
            redUnitTexture.texture = characterTexture[(int)character];

            if (redLastClickCharacterBtn != null)
                redLastClickCharacterBtn.image.color = btnDefualtColor;
            redCharacterBtn[(int)character].image.color = btnSelectedColor;
            redLastClickCharacterBtn = redCharacterBtn[(int)character];

            redCharacterSkillIcon[(int)character].SetActive(true);
            redLastSkillIcon = redCharacterSkillIcon[(int)character];
        }
    }

    public void Ready()
    {
        LobbyManager.Instance.ReadyGame();
    }

    public void Exit()
    {
        NetworkManager.Singleton.Shutdown();
        SceneLoader.Instance.LoadSceneAsync("MenuScene");
    }

    public void HideGuideText()
    {
        guideText.SetActive(false);
    }
}
