using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBaseEventSystem : MonoBehaviour
{
    public Transform monsterLocationParent;
    public List<Transform> monsterLocations;
    public bool isNightTime;

    private void Start()
    {
        SetUpDayMonsterLocation();
    }

    public void Update()
    {
        if (GameManager.instance.lightCycleController.isNightTime)
        {
            ActivateMonsterLocation();
        }
        else
        {
            DeactivateMonsterLocation();
        }
    }


    public void SetUpDayMonsterLocation()
    {
        foreach(Transform t in monsterLocationParent)
        {
            monsterLocations.Add(t);
        }
        DeactivateMonsterLocation();
    }

    public void ActivateMonsterLocation()
    {
        foreach (Transform t in monsterLocations)
        {
            if (!t.GetComponent<SummonTower>())
            {
                continue;
            }
            t.GetComponent<SummonTower>().enabled = true;
          
        }
    }

    public void DeactivateMonsterLocation()
    {
        foreach (Transform t in monsterLocations)
        {
            if (!t.GetComponent<SummonTower>())
            {
                continue;
            }
            t.GetComponent<SummonTower>().enabled = false;
        }
        GameManager.instance.enemyOverAllControl.DestroyAllMonsterInScene();
    }


}
