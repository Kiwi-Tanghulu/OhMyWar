using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSkillHandler : PlayerComponent
{
    [SerializeField] InputReader inputReader;
    [SerializeField] List<SkillBase> skills;

    public override void Init(Player player)
    {
        base.Init(player);

        if(player.IsOwner == false)
            return;
        
        inputReader.OnSkill1Pressed += HandleSkill1;
        inputReader.OnSkill2Pressed += HandleSkill2;
    }

    public override void Release()
    {
        if(player.IsOwner == false)
            return;
        
        inputReader.OnSkill1Pressed -= HandleSkill1;
        inputReader.OnSkill2Pressed -= HandleSkill2;    
    }

    public void HandleSkill(int index)
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
        SkillServerRPC(index);
    }

    [ServerRpc]
    private void SkillServerRPC(int index)
    {
        skills[index]?.Operate(player);
    }

    private void HandleSkill1()
    {
        HandleSkill(1);
    }

    private void HandleSkill2()
    {
        HandleSkill(2);
    }
}
