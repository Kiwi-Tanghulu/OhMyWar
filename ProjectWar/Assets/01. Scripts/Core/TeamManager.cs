using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance;

    [field: SerializeField]
    public LayerMask RedLayer { get; private set; }
    [field: SerializeField]
    public LayerMask BlueLayer { get; private set; }
    [field: SerializeField]
    public LayerMask NeutralityLayer { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ChangeTeam(GameObject changeedObj, GameObject changingObj)
    {
        changeedObj.layer = changingObj.layer;
    }

    public LayerMask GetEnemyLayer(int layer)
    {
        LayerMask myLayer = 1 << (layer - 1);

        return myLayer == BlueLayer ? RedLayer : BlueLayer;
    }
}
