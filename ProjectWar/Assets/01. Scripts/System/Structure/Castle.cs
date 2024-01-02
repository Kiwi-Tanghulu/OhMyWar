using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Castle : StructureBase
{
	[SerializeField] List<Transform> spawnPositions = null;
    [SerializeField] NetworkPrefabsList unitPrefabs = null;

    private ulong ownerID = 0;
    public ulong OwnerID => ownerID;

    public void Init(ulong ownerID)
    {
        this.ownerID = ownerID;
    }

    private void SpawnUnit(int unitIndex, int spawnIndex)
    {
        GameObject unit = Instantiate(unitPrefabs.PrefabList[unitIndex].Prefab, spawnPositions[spawnIndex].position, Quaternion.identity);
        unit.GetComponent<NetworkObject>().Spawn();
    }

    public void GenerateUnit(int unitIndex, int spawnIndex)
    {
        GenerateUnitServerRPC(unitIndex, spawnIndex);
    }

    [ServerRpc]
    private void GenerateUnitServerRPC(int unitIndex, int spawnIndex)
    {
        SpawnUnit(unitIndex, spawnIndex);
    }
}
