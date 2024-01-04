using System;
using System.Collections.Generic;
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

    [SerializeField] List<Player> playerPrefabs;

    public Player OwnerPlayer;

    public Player BluePlayer {get; private set;} = null;
    public Player RedPlayer {get; private set;} = null;

    public IUnitSpawner CurrentSpawner { get; private set; } = null;
    public int FocusedLine { get; private set; } = 0;

    public Castle castle { get; private set; } = null;

    private NetworkVariable<float> startedTime = new NetworkVariable<float>();

    public List<Transform> BluePoint;
    public List<Transform> NexusPoint;
    public List<Transform> RedPoint;

    public void RegisterPlayer(Player player)
    {
        if(player.IsBlue)
            BluePlayer = player;
        else
            RedPlayer = player;

        if (IsServer)
        {
            castle = BlueCastle;
            BlueCastle.SetSightMask(true);
            RedCastle.SetSightMask(false);
        }
        else
        {
            castle = RedCastle;
            BlueCastle.SetSightMask(false);
            RedCastle.SetSightMask(true);
        }

    }

    public void ToggleCurrentSpawner(Player player, int lineIndex)
    {
        FocusedLine = lineIndex;

        bool isBlue = player.IsBlue;
        CurrentSpawner = isBlue ? BlueCastle : RedCastle;
        Debug.Log($"IsBlue : {isBlue} / Spawner : {(isBlue ? BlueCastle : RedCastle).name}");

        // if(IsServer)
        //     CurrentSpawner = BlueCastle;
        // else
        //     CurrentSpawner = RedCastle;

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

    // 서버만 호출하는 함수
    public void StartGame(CharacterType blueType, CharacterType redType)
    {
        startedTime.Value = Time.time;

        BluePlayer = Instantiate(playerPrefabs[(int)blueType]);
        BluePlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(GameManager.Instance.HostID.Value);
        
        RedPlayer = Instantiate(playerPrefabs[(int)redType]);
        RedPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(GameManager.Instance.GuestID.Value);
    }

    public void CloseGame(ulong winnerID)
    {
        EndGameServerRPC(winnerID);
    }

    private void EndGame(bool isWin)
    {
        float gameTime = Time.time - startedTime.Value;
        int earnedGold = OwnerPlayer.TotalGold;
    }

    [ServerRpc]
    private void EndGameServerRPC(ulong winnerID)
    {
        EndGameClientRPC(winnerID);
    }

    [ClientRpc]
    private void EndGameClientRPC(ulong winnerID)
    {
        bool isWin = NetworkManager.Singleton.LocalClientId == winnerID;
        EndGame(isWin);
    }
}
