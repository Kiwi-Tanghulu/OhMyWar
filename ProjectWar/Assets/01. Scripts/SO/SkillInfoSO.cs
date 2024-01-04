using UnityEngine;

[CreateAssetMenu(menuName = "SO/SkillInfo")]
public class SkillInfoSO : ScriptableObject
{
    public string skillName;
    [TextArea] public string skillContent;
    public Sprite skillIcon;
}