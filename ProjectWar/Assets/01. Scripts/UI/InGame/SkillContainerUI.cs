using Unity.Netcode;
using UnityEngine;

public class SkillContainerUI : PanelUI
{
	[SerializeField] InputReader inputReader;

    private Player player = null;
    private Player Player {
        get {
            if(player == null)
                player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>();
            return player;
        }
    }

    private PlayerSkillHandler skillHandler = null;
    public PlayerSkillHandler SkillHandler {
        get {
            if(skillHandler == null)
                skillHandler = Player.GetComponent<PlayerSkillHandler>();
            return skillHandler;
        }
    }

    public void HandleSkill(int index)
    {
        SkillHandler.HandleSkill(index);
    }
}
