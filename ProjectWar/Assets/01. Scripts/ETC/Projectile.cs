using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    private Collider2D target;
    [SerializeField] private float moveSpeed;
    [SerializeField]private Vector2 moveDir;

    public void Init(GameObject target, Vector2 movedir)
    {
        moveDir = movedir;
        this.target = target.GetComponent<Collider2D>();
        transform.right = moveDir;
    }

    private void Update()
    {
        transform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == target)
            Destroy(gameObject);
    }
}
