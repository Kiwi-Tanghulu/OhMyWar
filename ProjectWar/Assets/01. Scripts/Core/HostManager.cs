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
    private string joinCode;

    private const int MaxConnection = 8;
    
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

        OnRoomCreatedEvent?.Invoke();
    }
}
