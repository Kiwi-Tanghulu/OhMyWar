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
    
    private void Start()
    {
        if(PlayerPrefs.GetString("PlayerID", null) != null)
            gameObject.SetActive(false);
    }

    public void IsAble()
    {
        button.interactable = !(inputField.text.Split(' ').Length <= 0 || inputField.text.Length > length);
    }

    public void SetNickname()
    {
        Debug.Log($"{inputField.text} / {inputField.text.Split(' ').Length}");
        if(inputField.text.Split(' ').Length <= 0)
            return;

        if(inputField.text.Length <= 0 || inputField.text.Length > length)
            return;

        PlayerPrefs.SetString("PlayerID", inputField.text);
        OnSetEvent?.Invoke();
    }
}
