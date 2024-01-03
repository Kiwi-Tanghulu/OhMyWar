using System;
using UnityEngine;

public class PlayerMovement : PlayerComponent
{
    [SerializeField] InputReader inputReader = null;

    [Space(10f)]
    [SerializeField] float speed = 10f;

    private Transform visual = null;

    private Vector2 moveDirection = Vector2.zero;

    private Vector2 targetPosition = Vector2.zero;
    public Vector2 TargetPosition => targetPosition;

    private int facingDirection = 0;
    public int FacingDirection => (visual.transform.eulerAngles.y == 0f ? 1 : -1);

    private bool isMove = false;
    private PlayerAnimator playerAnimator;

    public override void Init(Player player)
    {
        base.Init(player);
    
        visual = transform.Find("Visual");

        if(player.IsOwner)
            inputReader.OnRightClicked += HandleRightClicked;

        playerAnimator = visual.GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        if(isMove)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(inputReader.MousePosition);
            SetTargetPosition(worldPos);
        }
    }

    private void FixedUpdate()
    {
        moveDirection = targetPosition - (Vector2)transform.position;
        if (moveDirection.sqrMagnitude < 0.1f)
        {
            if(player.IsOwner)
                playerAnimator.AServerRPC(false);
            return;
        }

        moveDirection.Normalize();

        DoRotate();
        DoMove();
    }

    public override void Release()
    {
        if(player.IsOwner)
            inputReader.OnRightClicked -= HandleRightClicked;
    }

    private void DoRotate()
    {
        facingDirection = (int)Mathf.Sign(moveDirection.x);

        float angle = (facingDirection == 1) ? 0f : 180f;
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
        // visual.rotation = Quaternion.Lerp(visual.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        visual.rotation = targetRotation;
    }

    private void DoMove()
    {
        Vector3 moveVector = moveDirection * speed * Time.fixedDeltaTime;
        transform.position += moveVector;
    }

    private void HandleRightClicked(bool value)
    {
        isMove = value;

    }

    public void SetTargetPosition(Vector2 position)
    {
        targetPosition = position;
        if(player.IsOwner)
                playerAnimator.AServerRPC(true);
    }


    public void MoveImmediately(Vector2 position)
    {
        SetTargetPosition(position);
        transform.position = position;
    }
}
