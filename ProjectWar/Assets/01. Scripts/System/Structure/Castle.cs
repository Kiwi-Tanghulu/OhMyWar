using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Castle : StructureBase, IUnitSpawner
{
	[SerializeField] List<Transform> spawnPositions = null;
	[SerializeField] List<Nexus> midNexus = null;
    [SerializeField] Transform spawnPosition = null;
    public Transform SpawnPosition => spawnPosition;


    [Space(10f)]
    [SerializeField] NetworkPrefabsList unitPrefabs = null; 

    private GameObject sightMask = null;

    private ulong ownerID = 0;
    public ulong OwnerID => ownerID;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        sightMask = transform.Find("CastleSightMask").gameObject;
    }

    public override void OnDamaged(int damage = 0, NetworkObject performer = null, Vector3 point = default)
    {
        if (performer.OwnerClientId == NetworkManager.Singleton.LocalClientId)
        {
            sightMask.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(DelayCoroutine(4f, () => sightMask.SetActive(false)));
        }

        base.OnDamaged(damage, performer, point);
    }

    public void Init(ulong ownerID)
    {
        this.ownerID = ownerID;
    }

    public void SpawnUnit(int unitIndex, int lineIndex)
    {
        if (unitIndex >= unitPrefabs.PrefabList.Count)
            return;
        int pointIndex = 0;
        if (1 << gameObject.layer == TeamManager.Instance.RedLayer)
            pointIndex = 2;

        UnitManager.Instance.SpawnUnit((UnitType)unitIndex, NetworkManager.LocalClientId,
            lineIndex, pointIndex);
    }

    public override void OnDie(NetworkObject performer)
    {
        base.OnDie(performer);
        Debug.Log("die1");
        IngameManager.Instance.CloseGame(performer.OwnerClientId);
    }

    public void SetSightMask(bool value)
    {
        sightMask.SetActive(value);
    }
}
