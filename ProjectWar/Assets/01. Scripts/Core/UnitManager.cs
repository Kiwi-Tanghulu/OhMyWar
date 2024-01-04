using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : NetworkBehaviour
{
    public static UnitManager Instance;

    [SerializeField] private List<UnitController> units;
    private Dictionary<UnitType, UnitController> unitDictionary;
    private Dictionary<ulong, List<UnitController>> playerUnitContainer;

    [field: SerializeField]
    public string RedUnitTag { get; private set; } = "RedUnit";
    [field: SerializeField]
    public string BlueUnitTag { get; private set; } = "BlueUnit";

    public event Action UnitSpawnEvent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        unitDictionary = new();
        playerUnitContainer = new();

        foreach(UnitController unit in units)
        {
            unitDictionary.Add(unit.Info.unitType, unit);
        }
    }

    public void SpawnUnit(UnitType type, ulong clientId, Vector2 spawnPosition, Vector2 targetPosition)
    {
        Vector2 offset = new Vector2(0, UnityEngine.Random.Range(-2f, 2f));
        UnitSpawnEvent?.Invoke();
        SpawnUnitServerRpc(type, clientId, spawnPosition, targetPosition, offset);
    }

    public void DespawnUnit(UnitController unit)
    {
        unit.GetComponent<NetworkObject>().Despawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnUnitServerRpc(UnitType type, ulong clientId, Vector2 spawnPosition, Vector2 targetPosition, Vector2 offset)
    {
        UnitController unit = Instantiate(unitDictionary[type], spawnPosition + offset, Quaternion.identity);
        NetworkObject unitNetworkObject = unit.GetComponent<NetworkObject>();
        unitNetworkObject.SpawnWithOwnership(clientId, true);

        Player player = null;
        if(clientId == GameManager.Instance.HostID.Value)
            player = IngameManager.Instance.BluePlayer;
        else
            player = IngameManager.Instance.RedPlayer;

        player.Buffs.ForEach(i => unit.Stat.AddModifier(i.type, i.value));

        if (!playerUnitContainer.ContainsKey(clientId))
            playerUnitContainer.Add(clientId, new());

        playerUnitContainer[clientId].Add(unit);
        unit.SetOffset(offset);
        unit.Movement.SetTargetPosition(targetPosition);
    }
}
