using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Castle : StructureBase, IUnitSpawner
{
	[SerializeField] List<Transform> spawnPositions = null;
    [SerializeField] Transform spawnPosition = null;
    public Transform SpawnPosition => spawnPosition;


    [Space(10f)]
    [SerializeField] NetworkPrefabsList unitPrefabs = null; 

    private ulong ownerID = 0;
    public ulong OwnerID => ownerID;

    public void Init(ulong ownerID)
    {
        this.ownerID = ownerID;
    }

    public void SpawnUnit(int unitIndex, int lineIndex)
    {
        GenerateServerRPC(unitIndex, lineIndex);
    }

    private void GenerateUnit(int unitIndex, int lineIndex)
    {
        if(unitIndex >= unitPrefabs.PrefabList.Count)
            return;

        GameObject unit = Instantiate(unitPrefabs.PrefabList[unitIndex].Prefab, spawnPosition.position, Quaternion.identity);
        unit.GetComponent<NetworkObject>().Spawn();
        unit.GetComponent<UnitController>().Movement.SetTargetPosition(spawnPosition.position);
    }
    
    [ServerRpc]
    private void GenerateServerRPC(int unitIndex, int lineIndex)
    {
        GenerateUnit(unitIndex, lineIndex);
    }
}
