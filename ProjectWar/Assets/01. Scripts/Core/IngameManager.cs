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

    public Transform OwnerPlayer;

    private Player bluePlayer;
    private Player redPlayer;

    public IUnitSpawner CurrentSpawner { get; private set; } = null;
    public int FocusedLine { get; private set; } = 0;

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

    public void ToggleCurrentSpawner(Player player, int lineIndex)
    {
        FocusedLine = lineIndex;

        bool isBlue = player == bluePlayer;
        CurrentSpawner = isBlue ? BlueCastle : RedCastle;

        if(lineIndex == 0) // top
            CheckNexus(TopNexus, player);
        else if(lineIndex == 1) // mid
            CheckNexus(MidNexus, player);
        else if(lineIndex == 2) // bottom
            CheckNexus(BottomNexus, player);
    }

    private void CheckNexus(Nexus nexus, Player player)
    {
        if(nexus.OwnerID == player.OwnerClientId)
            CurrentSpawner = nexus;
    }

    public void SpawnUnit(int unitIndex)
    {
        CurrentSpawner?.SpawnUnit(unitIndex, FocusedLine);
    }
}
