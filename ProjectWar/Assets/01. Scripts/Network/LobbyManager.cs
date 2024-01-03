using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    private LobbyPanel lobbyPanel;

    public override void OnNetworkSpawn()
    {
        Transform lobbyPanelTrm = UIManager.Instance.MainCanvas.Find("LobbyPanel");
        lobbyPanel = lobbyPanelTrm.GetComponent<LobbyPanel>();
        if (IsHost)
            lobbyPanel.Init(UserType.Blue,this);
        else
        {
            lobbyPanel.Init(UserType.Red,this);

        }
        NetworkManager.Singleton.OnClientConnectedCallback += lobbyPanel.ShowClientPanel;
        NetworkManager.Singleton.OnClientDisconnectCallback += lobbyPanel.HideClientPanel;
    }

    [ClientRpc]
    public void ClientDisConnectClientRPC(ulong id)
    {

    }

    [ServerRpc]
    public void CharacterButtonPressServerRPC(UserType user, CharacterType character)
    {
        ChangeUIInfoClientRPC(user, character);
    }
    [ClientRpc]
    public void ChangeUIInfoClientRPC(UserType user, CharacterType character)
    {
        lobbyPanel.SettingPaenlInfo(user, character);
    }
}
