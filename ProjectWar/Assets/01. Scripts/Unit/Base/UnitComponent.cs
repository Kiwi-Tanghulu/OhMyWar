using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class UnitComponent : NetworkBehaviour
{
    protected UnitController controller;

    public virtual void InitCompo(UnitController _controller)
    {
        controller = _controller;
    }
}
