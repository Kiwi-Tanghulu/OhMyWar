using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    private static LobbyManager instance = null;
    public static LobbyManager Instance {
        get {
            if(instance == null)
                instance = FindObjectOfType<LobbyManager>();
            return instance;
        }
    }

    private LobbyPanel lobbyPanel;

    private bool otherReady = false;

    public override void OnNetworkSpawn()
    {
        Transform lobbyPanelTrm = GameObject.Find("LobbyPanel").transform;
        lobbyPanel = lobbyPanelTrm.GetComponent<LobbyPanel>();
        if (IsHost)
            lobbyPanel.Init(UserType.Blue, this);
        else
            lobbyPanel.Init(UserType.Red, this);

        NetworkManager.Singleton.OnClientConnectedCallback += lobbyPanel.ShowClientPanel;
        NetworkManager.Singleton.OnClientDisconnectCallback += lobbyPanel.HideClientPanel;
    }

    public override void OnNetworkDespawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= lobbyPanel.ShowClientPanel;
        NetworkManager.Singleton.OnClientDisconnectCallback -= lobbyPanel.HideClientPanel;
    }

    [ClientRpc]
    public void ClientDisconnectClientRPC(ulong id)
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

    public void ReadyGame()
    {
        // UI
        ReadyServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ReadyServerRPC()
    {
        Debug.Log(otherReady);
        if(otherReady)
            StartGame();
        else
            otherReady = true;
        Debug.Log(otherReady);
    }

    private void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += HandleLoadCompleted;
    }

    private void HandleLoadCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        IngameManager.Instance.StartGame();
    }
}
