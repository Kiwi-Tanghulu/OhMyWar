using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightObject : MonoBehaviour
{
    [SerializeField] private GameObject minimapPoint;
    private bool isSight;

    private void Awake()
    {
        minimapPoint = transform.Find("MinimapPoint").gameObject;
        isSight = false;
    }

    public void SetSight(bool value)
    {
        if (isSight == value)
            return;

        isSight = value;
        minimapPoint.SetActive(value);
    }
}
