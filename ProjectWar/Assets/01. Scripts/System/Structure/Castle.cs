using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Castle : StructureBase
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

    public void SpawnUnit(int unitIndex, int spawnIndex)
    {
        GameObject unit = Instantiate(unitPrefabs.PrefabList[unitIndex].Prefab, spawnPositions[spawnIndex].position, Quaternion.identity);
        unit.GetComponent<NetworkObject>().Spawn();
        unit.GetComponent<UnitController>().Movement.SetTargetPosition(spawnPositions[spawnIndex].position);
    }
}
