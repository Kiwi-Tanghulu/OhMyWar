using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitManager : NetworkBehaviour
{
    public static UnitManager Instance;

    [SerializeField] private List<UnitController> units;
    private Dictionary<UnitType, UnitController> unitDictionary;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        foreach(UnitController unit in units)
        {
            unitDictionary.Add(unit.Info.unitType, unit);
        }
    }

    public void SpawnUnit(UnitType type, ulong clientId, Vector2 position)
    {
        SpawnUnitServerRpc(type, clientId, position);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnUnitServerRpc(UnitType type, ulong clientId, Vector2 position)
    {
        NetworkObject unit = Instantiate(unitDictionary[type], position, Quaternion.identity).GetComponent<NetworkObject>();
        unit.SpawnWithOwnership(clientId, true);
    }
}
