using UnityEngine;

public class TSkill : SkillBase
{
    protected override bool ActiveSkill()
    {
        Debug.Log("Skill Actived");
        return true;
    }
}
