using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LineArrow : NetworkBehaviour
{
    public void SetLineArrow(int focusedLine)
    {
        if (IsServer)
        {
            transform.position = IngameManager.Instance.BluePoint[focusedLine].position;
            transform.rotation = Quaternion.Euler(0, 0, 25f - 25 * focusedLine);
        }
        else
        {
            transform.position = IngameManager.Instance.RedPoint[focusedLine].position;
            transform.rotation = Quaternion.Euler(0, 0, 180f - 25f + 25 * focusedLine);
        }
    }
}
