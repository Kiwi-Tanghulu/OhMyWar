using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
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
