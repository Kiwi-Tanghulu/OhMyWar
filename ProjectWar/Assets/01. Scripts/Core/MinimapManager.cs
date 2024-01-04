using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MinimapManager : NetworkBehaviour
{
    public static MinimapManager Instance;

    [SerializeField] private List<ViewObject> viewObjects = new List<ViewObject>();
    [SerializeField] private List<SightObject> sightObjects = new List<SightObject>();
    [SerializeField] private float viewDelay;

    private WaitForSeconds wfs;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        wfs = new WaitForSeconds(viewDelay);
    }

    private void Start()
    {
        StartCoroutine(CheckView());
    }

    public void RegistViewObject(ViewObject obj)
    {
        viewObjects.Add(obj);
    }
    public void UnRegistViewObject(ViewObject obj)
    {
        viewObjects.Remove(obj);
    }

    public void RegistSightObject(SightObject obj)
    {
        sightObjects.Add(obj);
    }
    public void UnRegistSightObject(SightObject obj)
    {
        sightObjects.Remove(obj);
    }

    private IEnumerator CheckView()
    {
        while(true)
        {
            for(int i = 0; i < sightObjects.Count; i++)
            {
                bool isSight = false;

                for(int j = 0; j < viewObjects.Count; j++)
                {
                    if (Vector2.Distance(sightObjects[i].transform.position, viewObjects[i].transform.position) >= viewObjects[i].viewDistace)
                    {
                        isSight = true;
                        break;
                    }
                }

                sightObjects[i].SetSight(isSight);
            }

            yield return wfs;
        }
    }
}
