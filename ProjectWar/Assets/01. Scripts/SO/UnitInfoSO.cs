using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using Unity.Netcode;
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
    public NetworkObject projectile;
    public LayerMask targetLayer;
    public ParticleSystem attackEffect;

    protected Dictionary<UnitStatType, FieldInfo> fieldInfoDictionary;

    public void IncreaseStat(UnitStatType statType, int value)
    {
        switch (statType)
        {
            case UnitStatType.moveSpeed:
                moveSpeed += value;
                break;
            case UnitStatType.maxHealth:
                maxHealth += value;
                break;
            case UnitStatType.attackDamage:
                attackDamage += value;
                break;
            case UnitStatType.attackDistance:
                attackDistance += value;
                break;
            case UnitStatType.attackDelay:
                attackDelay += value;
                break;
            case UnitStatType.serchDelay:
                serchDelay += value;
                break;
        }
    }
}