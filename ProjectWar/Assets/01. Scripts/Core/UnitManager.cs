using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
        SpawnUnitServerRpc(type, clientId, spawnPosition, targetPosition);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnUnitServerRpc(UnitType type, ulong clientId, Vector2 spawnPosition, Vector2 targetPosition)
    {
        UnitController unit = Instantiate(unitDictionary[type], spawnPosition, Quaternion.identity);
        NetworkObject unitNetworkObject = unit.GetComponent<NetworkObject>();
        unitNetworkObject.SpawnWithOwnership(clientId, true);

        if (!playerUnitContainer.ContainsKey(clientId))
            playerUnitContainer.Add(clientId, new());

        playerUnitContainer[clientId].Add(unit);
        unit.Movement.SetTargetPosition(targetPosition);
    }
}
