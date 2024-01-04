using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightObject : MonoBehaviour
{
    [SerializeField] private GameObject minimapPoint;
    private bool isSight;

    private void Start()
    {
        isSight = false;
        SetSight(false);

        MinimapManager.Instance.RegistSightObject(this);
    }

    public void SetSight(bool value)
    {
        isSight = value;
        minimapPoint.SetActive(value);
    }
}
