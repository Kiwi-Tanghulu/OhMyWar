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

    private CharacterType blueCharacterType;
    private CharacterType redCharacterType;

    public override void OnNetworkSpawn()
    {
        Transform lobbyPanelTrm = GameObject.Find("LobbyPanel").transform;
        lobbyPanel = lobbyPanelTrm.GetComponent<LobbyPanel>();
        if (IsHost)
        {
            lobbyPanel.Init(UserType.Blue, this);
            lobbyPanel.HideClientPanel(123);
            NetworkManager.Singleton.OnClientConnectedCallback += lobbyPanel.ShowClientPanel;
            NetworkManager.Singleton.OnClientDisconnectCallback += lobbyPanel.HideClientPanel;
        }
        else
        {
            lobbyPanel.Init(UserType.Red, this);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsHost)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= lobbyPanel.ShowClientPanel;
            NetworkManager.Singleton.OnClientDisconnectCallback -= lobbyPanel.HideClientPanel;
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void CharacterButtonPressServerRPC(UserType user, CharacterType character)
    {
        ChangeUIInfoClientRPC(user, character);

        if(user == UserType.Blue)
            blueCharacterType = character;
        else
            redCharacterType = character;
    }
    [ClientRpc]
    public void ChangeUIInfoClientRPC(UserType user, CharacterType character)
    {
        Debug.Log($"{user} {character}");
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
        if(otherReady)
            StartGame();
        else
            otherReady = true;
    }



    [ServerRpc]
    public void BlueReadyServerRPC()
    {
        BlueReadyClientRPC();
    }
    [ServerRpc(RequireOwnership = false)]
    public void RedReadyServerRPC()
    {
        RedReadyClientRPC();
    }
    [ClientRpc]
    public void BlueReadyClientRPC()
    {
        lobbyPanel.ChangeBlueUI();
    }
    [ClientRpc]
    public void RedReadyClientRPC()
    {
        lobbyPanel.ChangeRedUI();
    }

    private void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += HandleLoadCompleted;
    }

    private void HandleLoadCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        IngameManager.Instance.StartGame(blueCharacterType, redCharacterType);
    }

}
