using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string skillName = null;
    [SerializeField, TextArea] string skillContent = null;
    private SkillInfoPanel infoPanel = null;

    private void Awake()
    {
        Transform infoPanelTrm = transform.parent.parent.parent.Find("SkillInfoPanel");
        infoPanel = infoPanelTrm.GetComponent<SkillInfoPanel>();
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
}
