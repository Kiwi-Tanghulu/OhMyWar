using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffect : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Play()
    {
        anim.SetTrigger("Play");
    }

    public void End()
    {
        Destroy(gameObject);
    }
}
