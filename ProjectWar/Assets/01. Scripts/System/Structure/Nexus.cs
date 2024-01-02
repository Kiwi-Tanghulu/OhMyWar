using Unity.Netcode;
using UnityEngine;

public class Nexus : StructureBase
{
    [SerializeField] Transform spawnPosition = null;
    [SerializeField] NetworkPrefabsList unitPrefabs = null;

    private ulong ownerID = 0;
    public ulong OwnerID => ownerID;

    public void Init(ulong ownerID)
    {
        this.ownerID = ownerID;
    }
	
    private void SpawnUnit(int unitIndex)
    {
        GameObject unit = Instantiate(unitPrefabs.PrefabList[unitIndex].Prefab, spawnPosition.position, Quaternion.identity);
        unit.GetComponent<NetworkObject>().Spawn();
        // initialize unit here
    }

    public void GenerateUnit(int unitIndex)
    {
        GenerateUnitServerRPC(unitIndex);
    }

    [ServerRpc]
    private void GenerateUnitServerRPC(int unitIndex)
    {
        SpawnUnit(unitIndex);
    }

    public void ChangeOwner(NetworkClient networkCliet)
    {
        this.ownerID = networkCliet.ClientId;
        ModifyHP(maxHP);

        // change spawn position
    }
}
