using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : UnitComponent
{
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private Vector2 moveDir;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stopDistance;
    [SerializeField] private bool isArrived = false;

    public void SetTargetPosition(Vector2 pos)
    {
        if (!IsServer)
            return;

        isArrived = false;
        targetPosition = pos;
        moveDir = (targetPosition - (Vector2)transform.position).normalized;
    }

    public void Move()
    {
        if (!IsServer)
            return;

        if (isArrived)
            return;

        Vector3 moveVector = moveDir * moveSpeed * Time.deltaTime;

        transform.position += moveVector;

        if(Vector2.Distance(transform.position, targetPosition) <= stopDistance)
        {
            SetTargetPosition(transform.position);
            isArrived = true;
        }
    }
}
