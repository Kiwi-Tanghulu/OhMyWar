using Unity.Netcode;
using UnityEngine;

public class Nexus : StructureBase, IUnitSpawner
{
    [SerializeField] Transform spawnPosition = null;
    [SerializeField] NetworkPrefabsList unitPrefabs = null;

    [Space(10f)]
    [SerializeField] Sprite blueSprite = null;
    [SerializeField] Sprite redSprite = null;

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

    public void ChangeOwner(NetworkClient networkCliet)
    {
        this.ownerID = networkCliet.ClientId;
        ModifyHP(maxHP);

        // change spawn position
    }
}
