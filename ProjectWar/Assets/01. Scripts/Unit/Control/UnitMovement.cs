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
    [SerializeField] private Transform visualTrm;

    public bool ShouldMove => shouldMove;
    public bool IsArrived => isArrived;
    public int LineIndex { get; private set; } = 0;
    public int TargetPointIndex { get; private set; } = -1;

    public override void InitCompo(UnitController _controller)
    {
        base.InitCompo(_controller);

        this.moveSpeed = controller.Info.moveSpeed;
        this.stopDistance = controller.Info.stopDistance;

        visualTrm = transform.Find("Visual");

        controller.Stat.GetStat(UnitStatType.moveSpeed).OnValueChangeEvent += MoveSpeedValueChange;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        controller.Stat.GetStat(UnitStatType.moveSpeed).OnValueChangeEvent -= MoveSpeedValueChange;
    }

    public void SetTargetPosition(Vector2 pos)
    {
        targetPosition = pos + controller.Offset;
        moveDir = (targetPosition - (Vector2)transform.position).normalized;

        if (moveDir.x < 0)
            visualTrm.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if(moveDir.x > 0)
            visualTrm.rotation = Quaternion.Euler(0f, 180f, 0f);

        if (Vector2.Distance(transform.position, targetPosition) > stopDistance)
        {
            isArrived = false;
            shouldMove = true;
        }
    }

    public void SetLine(int lineIndex)
    {
        LineIndex = lineIndex;

        if(TargetPointIndex == -1)
        {
            TargetPointIndex = 1;
        }

        SetTargetPosition(IngameManager.Instance.GetLinePoint(lineIndex, TargetPointIndex).position);
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
            if (gameObject.tag == UnitManager.Instance.BlueUnitTag)
                TargetPointIndex++;
            else
                TargetPointIndex--;

            if(!(TargetPointIndex < 0 || TargetPointIndex > IngameManager.Instance.MaxLinePointIndex))
                SetTargetPosition(IngameManager.Instance.GetLinePoint(LineIndex, TargetPointIndex).position);
            else
                SetTargetPosition(transform.position);
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

    private void MoveSpeedValueChange(float value) => moveSpeed = value;
}
