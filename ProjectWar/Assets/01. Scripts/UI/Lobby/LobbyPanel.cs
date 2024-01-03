using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LobbyPanel : FixedUI
{
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

    public void Init(UserType user, LobbyManager lobbyManager)
    {
        myType = user;

        //if (user == UserType.Red)

        transform.Find($"{user}/KnightSelectBtn").GetComponent<Button>().onClick.AddListener(
            () => lobbyManager.CharacterButtonPressServerRPC(UserType.Blue, CharacterType.Knight));
        transform.Find($"{user}/PsychicSelectBtn").GetComponent<Button>().onClick.AddListener(
            () => lobbyManager.CharacterButtonPressServerRPC(UserType.Blue, CharacterType.Psychic));

        SettingPaenlInfo(myType, CharacterType.Knight);
    }

    public void ShowClientPanel(ulong id)
    {
        transform.Find("Red").gameObject.SetActive(true);
    }

    public void HideClientPanel(ulong id)
    {
        transform.Find("Red").gameObject.SetActive(false);
    }

    public void SettingPaenlInfo(UserType user, CharacterType character)
    {
        if (myType == user)
        {
            blueUnitTexture.texture = characterTexture[(int)character];

            blueCharacterBtn[(int)character].image.color = btnSelectedColor;
            blueLastClickCharacterBtn = blueCharacterBtn[(int)character];
            blueLastClickCharacterBtn.image.color = btnDefualtColor;

            blueCharacterSkillIcon[(int)character].SetActive(true);
            blueLastSkillIcon = blueCharacterSkillIcon[(int)character];
            blueLastSkillIcon.SetActive(false);
        }
        else
        {
            redUnitTexture.texture = characterTexture[(int)character];

            redCharacterBtn[(int)character].image.color = btnSelectedColor;
            redLastClickCharacterBtn = redCharacterBtn[(int)character];
            redLastClickCharacterBtn.image.color = btnDefualtColor;

            redCharacterSkillIcon[(int)character].SetActive(true);
            redLastSkillIcon = redCharacterSkillIcon[(int)character];
            redLastSkillIcon.SetActive(false);
        }
    }

    public void Ready()
    {
        LobbyManager.Instance.ReadyGame();
    }
}
