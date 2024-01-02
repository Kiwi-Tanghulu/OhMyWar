using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Castle : StructureBase, IUnitSpawner
{
	[SerializeField] List<Transform> spawnPositions = null;
	[SerializeField] List<Transform> midNexus = null;
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
        if (unitIndex >= unitPrefabs.PrefabList.Count)
            return;

        Debug.Log(spawnPositions[lineIndex].position);
        UnitManager.Instance.SpawnUnit((UnitType)unitIndex, NetworkManager.LocalClientId,
            spawnPositions[lineIndex].position, midNexus[lineIndex].position);
    }
}
