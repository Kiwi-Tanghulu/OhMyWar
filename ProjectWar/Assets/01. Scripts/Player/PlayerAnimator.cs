using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayMoveAnimation(bool value)
    {
        animator.SetBool("Move", value);
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    [ServerRpc]
    public void AServerRPC(bool value)
    {
        BClientRPC(value);
    }

    [ClientRpc]
    public void BClientRPC(bool value)
    {
        PlayMoveAnimation(value);
    }
}