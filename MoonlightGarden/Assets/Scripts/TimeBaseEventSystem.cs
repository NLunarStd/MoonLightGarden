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
        foreach (Transform monsterNest in monsterLocations)
        {
            if (!monsterNest.GetComponent<SummonTower>())
            {
                continue;
            }
            monsterNest.GetComponent<SummonTower>().enabled = true;
            monsterNest.GetComponent<SummonTower>().StartSummon();
        }
    }

    public void DeactivateMonsterLocation()
    {
        foreach (Transform monsterNest in monsterLocations)
        {
            if (!monsterNest.GetComponent<SummonTower>())
            {
                continue;
            }
            monsterNest.GetComponent<SummonTower>().enabled = false;
            foreach (Transform monster in monsterNest)
            {
                monster.transform.localPosition = Vector3.zero;
            }
            monsterNest.GetComponent<SummonTower>().StopSummon();
        }
        //GameManager.instance.enemyOverAllControl.DestroyAllMonsterInScene();
    }


}
