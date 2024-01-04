using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSlotUI2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] SkillInfoPanel2 infoPanel = null;

    [Space(10f)]
    [SerializeField] string skillName = null;
    [SerializeField, TextArea] string skillContent = null;
    [SerializeField] Sprite sprite = null;

    [SerializeField] SkillInfoSO data;

    private void Awake()
    {
        if (data != null)
        {
            skillName = data.skillName;
            skillContent = data.skillContent;
            sprite = data.skillIcon;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPanel.SetInfo(skillName, skillContent, sprite);
        infoPanel.Show(transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.Hide();
    }

    public void SetName(string name)
    {
        skillName = name;
    }

    public void SetContent(string content)
    {
        skillContent = content;
    }
}
