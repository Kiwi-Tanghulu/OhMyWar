using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class ClientManager
{
    public static ClientManager Instance = null;

	private JoinAllocation allocation = null;

    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();
        AuthState authState = await Authenticator.DoAuth();
        return (authState == AuthState.Authenticated);
    }

    public async Task JoinRoomAsync(string roomCode)
    {
        try 
        {
            allocation = await Relay.Instance.JoinAllocationAsync(roomCode);
        } 
        catch(Exception err)
        {
            Debug.Log(err.Message);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        // NetworkManager.Singleton.OnServerStopped += HandleServerStopped;
        NetworkManager.Singleton.OnClientStopped += HandleClientStopped;

        NetworkManager.Singleton.StartClient();
    }

    private void HandleClientStopped(bool isHosted)
    {
        if(isHosted == false)
            return;

        if(IngameManager.Instance != null && IngameManager.Instance.OnGaming)
            SceneLoader.Instance.LoadSceneAsync("MenuScene");
    }
}
