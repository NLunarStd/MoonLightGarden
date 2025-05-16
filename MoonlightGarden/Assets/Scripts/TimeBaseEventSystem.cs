using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBaseEventSystem : MonoBehaviour
{
    public Transform monsterLocationParent;
    public bool isNightTime;


    public void Update()
    {
        if(GameManager.instance.lightCycleController.isNightTime)
        {

        }
    }


    public void SetUpDayMonsterLocation()
    {

    }


}
