using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoPanel2 : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text contentText;
    [SerializeField] Image image;

    public void Show(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetInfo(string name, string content,Sprite icon)
    {
        nameText.text = name;
        contentText.text = content;
        image.sprite = icon;
    }
}
