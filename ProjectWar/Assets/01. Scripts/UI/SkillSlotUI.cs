using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] SkillInfoPanel infoPanel = null;

    [Space(10f)]
    [SerializeField] string skillName = null;
    [SerializeField, TextArea] string skillContent = null;

    [SerializeField] SkillInfoSO data;

    private void Awake()
    {
        if(data != null)
        {
            skillName = data.skillName;
            skillContent = data.skillContent;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPanel.SetInfo(skillName, skillContent);
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
