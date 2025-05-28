using UnityEngine;
using System.Collections.Generic;
using System;
public class EnemyOverAllControl : MonoBehaviour
{
    
    public List<GameObject> monsterInScene = new List<GameObject>();


    public void SetAllToStop()
    {
        foreach (GameObject monster in monsterInScene)
        {
            if (monster.GetComponent<Monster>() != null) monster.GetComponent<Monster>().enabled = false;
            if (monster.GetComponent<EnemyBehhaviour>() != null)  monster.GetComponent<EnemyBehhaviour>().enabled = false;
            if (monster.GetComponent<AIMove>() != null)  monster.GetComponent<AIMove>().enabled = false;
        }
    }

    public void DestroyAllMonsterInScene()
    {
        foreach (GameObject monster in monsterInScene)
        {
            if (monster.GetComponent<Monster>() != null) monster.GetComponent<Monster>().enabled = true;
            if (monster.GetComponent<EnemyBehhaviour>() != null) monster.GetComponent<EnemyBehhaviour>().enabled = true;
            if (monster.GetComponent<AIMove>() != null) monster.GetComponent<AIMove>().enabled = true;
            if (monster.GetComponent<Monster>() != null) monster.GetComponent<Monster>().TakeDamage(99999);
        }
    }

}
