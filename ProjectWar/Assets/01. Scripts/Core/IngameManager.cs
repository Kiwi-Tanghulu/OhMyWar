using Unity.Netcode;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    private static IngameManager instance;
    public static IngameManager Instance {
        get {
            if(instance == null)
                instance = FindObjectOfType<IngameManager>();
            return instance;
        }
    }

    [SerializeField] Castle blueCastle = null;
    [SerializeField] Castle redCastle = null;

    [Space(10f)]
    [SerializeField] Nexus topNexus = null;
    [SerializeField] Nexus midNexus = null;
    [SerializeField] Nexus bottomNexus = null;

    private NetworkClient player1 = null;
    private NetworkClient player2 = null;

    public void ReadyGame()
    {
        ReadyServerRPC(NetworkManager.Singleton.LocalClient, NetworkManager.Singleton.IsHost);
    }

    public void CloseGame(Castle loser)
    {

    }

    [ServerRpc]
    private void ReadyServerRPC(NetworkClient player, bool isHost)
    {
        if(isHost)
            player1 = player;
        else
            player2 = player;

        if(player1 != null && player2 != null)
            Debug.Log("Start Game");
    }
}
