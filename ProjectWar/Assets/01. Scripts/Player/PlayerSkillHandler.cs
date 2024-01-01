using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : PlayerComponent
{
    [SerializeField] InputReader inputReader;
    [SerializeField] List<SkillBase> skills;

    public override void Init(Player player)
    {
        base.Init(player);

        if(player.IsOwner)
            inputReader.OnNumberKeyPressed += HandleSkill;
    }

    public override void Release()
    {
        if(player.IsOwner)
            inputReader.OnNumberKeyPressed -= HandleSkill;
    }

    private void HandleSkill(int index)
    {
        if(index == 0)
            index = 10;

        index--;
        if(skills.Count < index)
            return;

        SkillBase skill = skills[index];
        if(player.Gold < skill.Cost)
            return;

        player.ModifyGold(-skill.Cost);
        skill?.Operate(player);
    }
}
