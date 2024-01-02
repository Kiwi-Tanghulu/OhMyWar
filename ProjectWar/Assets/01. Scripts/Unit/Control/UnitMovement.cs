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
    [SerializeField] private bool shouldMove = false;

    public bool ShouldMove => shouldMove;
    public bool IsArrived => isArrived;

    public void SetTargetPosition(Vector2 pos)
    {
        if (!IsServer)
            return;

        targetPosition = pos;
        moveDir = (targetPosition - (Vector2)transform.position).normalized;

        if (Vector2.Distance(transform.position, targetPosition) > stopDistance)
        {
            isArrived = false;
            shouldMove = true;
        }
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
            Stop();
        }
    }

    public void Stop()
    {
        if (!IsServer)
            return;

        SetTargetPosition(transform.position);
        isArrived = true;
        shouldMove = false;
    }
}
