using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseUI : BaseUI
{
    [SerializeField] private Transform target;

    public void RegisterTarget(Transform target)
    {
        this.target = target;
    }
    private void Update()
    {
        if (target != null)
            transform.position = target.position + Vector3.up;
    }
    public void ClearTarget()
    {
        target = null;
    }
}
