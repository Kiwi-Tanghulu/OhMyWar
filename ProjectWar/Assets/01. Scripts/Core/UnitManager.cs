using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

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

    public UnitDeadEffect deadObject;

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

    public void SpawnUnit(UnitType type, ulong clientId, int lineIndex, int pointIndex)
    {
        Player player = IngameManager.Instance.OwnerPlayer;
        if(player.Gold >= (int)unitDictionary[type].Info.cost)
        {
            Vector2 offset = new Vector2(0, UnityEngine.Random.Range(-1.5f, 1.5f));
            UnitSpawnEvent?.Invoke();
            SpawnUnitServerRpc(type, clientId, lineIndex, pointIndex, offset);
            player.ModifyGold(-(int)unitDictionary[type].Info.cost);
        }
    }

    public void DespawnUnit(UnitController unit)
    {
        //MinimapManager.Instance.UnRegistSightObject(unit.GetComponent<SightObject>());
        //MinimapManager.Instance.UnRegistViewObject(unit.GetComponent<ViewObject>());
        Instantiate(deadObject, unit.transform.position, Quaternion.identity, unit.transform).SetUnit(unit);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnUnitServerRpc(UnitType type, ulong clientId, int lineIndex, int pointIndex, Vector2 offset)
    {
        List<Transform> linePoints = IngameManager.Instance.linePoints[lineIndex].points;

        UnitController unit = Instantiate(unitDictionary[type], linePoints[pointIndex].position + (Vector3)offset, Quaternion.identity);
        NetworkObject unitNetworkObject = unit.GetComponent<NetworkObject>();
        unitNetworkObject.SpawnWithOwnership(clientId, true);

        Player player = null;
        if(clientId == GameManager.Instance.HostID.Value)
        {
            player = IngameManager.Instance.BluePlayer;
            unit.transform.position = linePoints[0].position + (Vector3)offset;
            unit.team = TeamType.Blue;
        }
        else
        {
            player = IngameManager.Instance.RedPlayer;
            unit.transform.position = linePoints[linePoints.Count - 1].position + (Vector3)offset;
            unit.team = TeamType.Red;
        }

        player.Buffs.ForEach(i => unit.Stat.AddModifier(i.type, i.value));

        if (!playerUnitContainer.ContainsKey(clientId))
            playerUnitContainer.Add(clientId, new());

        playerUnitContainer[clientId].Add(unit);
        unit.SetOffset(offset);
        unit.Movement.SetLine(lineIndex);
    }
}
