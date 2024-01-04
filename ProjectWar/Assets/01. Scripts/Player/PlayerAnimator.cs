using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = transform.parent.GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    public void PlayMoveAnimation(bool value)
    {
        animator.SetBool("Move", value);
    }

    public void PlaySkillkAnimation()
    {
        animator.SetTrigger("Skill");
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
    public void AnimationEndTrigger()
    {
        playerMovement.SetMoveable(true);
    }
}
