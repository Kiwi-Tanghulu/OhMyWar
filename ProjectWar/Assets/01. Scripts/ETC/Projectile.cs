using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : NetworkBehaviour
{
    private Collider2D target;
    [SerializeField] private float moveSpeed;
    [SerializeField]private Vector2 moveDir;

    public void Init(GameObject target, Vector2 movedir)
    {
        if (!IsServer)
            return;

        moveDir = movedir;
        this.target = target.GetComponent<Collider2D>();
        transform.right = moveDir;
    }

    private void Update()
    {
        if (!IsServer)
            return;

        transform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);
    }
}
