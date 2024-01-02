using Unity.Netcode;
using UnityEngine;

public class IngameManager : NetworkBehaviour
{
    private static IngameManager instance;
    public static IngameManager Instance {
        get {
            if(instance == null)
                instance = FindObjectOfType<IngameManager>();
            return instance;
        }
    }

    [field: SerializeField] public Castle BlueCastle { get; private set; } = null;
    [field: SerializeField] public Castle RedCastle { get; private set; } = null;

    [field: SerializeField] public Nexus TopNexus { get; private set; } = null;
    [field: SerializeField] public Nexus MidNexus { get; private set; } = null;
    [field: SerializeField] public Nexus BottomNexus { get; private set; } = null;

    private Player bluePlayer;
    private Player redPlayer;

    public void RegisterPlayer(Player player, bool isBluePlayer)
    {
        if(isBluePlayer)
        {
            bluePlayer = player;
            player.GetComponent<PlayerMovement>().MoveImmediately(BlueCastle.SpawnPosition.position);
        }
        else
        {
            redPlayer = player;
            player.GetComponent<PlayerMovement>().MoveImmediately(RedCastle.SpawnPosition.position);
        }
    }
}
