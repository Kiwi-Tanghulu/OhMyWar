using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/UnitInfo")]
public class UnitInfoSO : ScriptableObject
{
    [Header("Info")]
    public UnitType unitType;
    public int cost;
    public Sprite image;

    [Header("Movement")]
    public float moveSpeed;
    public float stopDistance;

    [Space]
    [Header("Health")]
    public float maxHealth;

    [Space]
    [Header("Attack")]
    public float attackDamage;
    public float attackDistance;
    public float attackDelay;
    public float serchDelay;
    public LayerMask targetLayer;
}
