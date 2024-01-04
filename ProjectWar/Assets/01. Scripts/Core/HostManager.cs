using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class HostManager
{
	public static HostManager Instance = null;

    public event Action OnRoomCreatedEvent;

    private Allocation allocation;
    public string joinCode { get; private set; }

    private const int MaxConnection = 2;
    
    public async Task CreateRoomAsync()
    {
        try 
        {
            allocation = await Relay.Instance.CreateAllocationAsync(MaxConnection);
        }
        catch(Exception err)
        {
            Debug.Log(err.Message);
            return;
        }

        try
        {
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            GUIUtility.systemCopyBuffer = joinCode;
            Debug.Log(joinCode);
        }
        catch(Exception err)
        {
            Debug.Log(err.Message);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
        GameManager.Instance.HostID.Value = NetworkManager.Singleton.LocalClientId;

        OnRoomCreatedEvent?.Invoke();
    }

    private void HandleClientDisconnected(ulong id)
    {
        if(IngameManager.Instance != null && IngameManager.Instance.OnGaming)
        {
            CloseHost();
            SceneLoader.Instance.LoadSceneAsync("MenuScene");
        }
    }

    private void HandleClientConnected(ulong id)
    {
        GameManager.Instance.GuestID.Value = id;
    }

    public void CloseHost()
    {
        NetworkManager.Singleton.Shutdown();
    }
}
