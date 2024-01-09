using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NicknamePanel : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button button;
    [SerializeField] int length = 10;

    [Space(10f)]
    [SerializeField] UnityEvent OnSetEvent;

    public void IsAble()
    {
        button.interactable = !(inputField.text.Split(' ').Length <= 0 || inputField.text.Length > length);
    }

    public void SetNickname()
    {
        if(inputField.text.Split(' ').Length <= 0)
            return;

        if(inputField.text.Length <= 0 && inputField.text.Length > length)
            return;

        PlayerPrefs.SetString("PlayerID", inputField.text);
        OnSetEvent?.Invoke();
    }
}
