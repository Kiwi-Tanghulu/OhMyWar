using TMPro;
using UnityEngine;

public class SkillInfoPanel : MonoBehaviour
{
	[SerializeField] TMP_Text nameText;
	[SerializeField] TMP_Text contentText;

    public void Show(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetInfo(string name, string content)
    {
        nameText.text = name;
        contentText.text = content;
    }
}
