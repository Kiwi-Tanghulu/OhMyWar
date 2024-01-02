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

    [SerializeField] private Player player1 = null;
    [SerializeField] private Player player2 = null;

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += ReadyGameServerRPC;
    }

    public void CloseGame(Castle loser)
    {

    }

    [ServerRpc]
    private void ReadyGameServerRPC(ulong playerID)
    {
        NetworkClient client = NetworkManager.Singleton.ConnectedClients[playerID];
        Player player = client.PlayerObject.GetComponent<Player>();
        bool isHost = player.IsHost && player.IsOwner;

        if(isHost)
            player1 = player;
        else
            player2 = player;

        if(player1 != null && player2 != null)
            Debug.Log("Start Game");
    }
}
