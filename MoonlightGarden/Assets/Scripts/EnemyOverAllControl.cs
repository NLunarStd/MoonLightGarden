using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
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
        /*
        foreach (GameObject monster in monsterInScene)
        {
            if (monster.GetComponent<Monster>() != null) monster.GetComponent<Monster>().enabled = true;
            if (monster.GetComponent<EnemyBehhaviour>() != null) monster.GetComponent<EnemyBehhaviour>().enabled = true;
            if (monster.GetComponent<AIMove>() != null) monster.GetComponent<AIMove>().enabled = true;
            if (monster.GetComponent<Monster>() != null) monster.GetComponent<Monster>().TakeDamage(99999);
        }
        */
        while (monsterInScene.Count > 0)
        {
            GameObject monster = monsterInScene[0];
            if (monster.GetComponent<Monster>() != null) monster.GetComponent<Monster>().enabled = true;
            if (monster.GetComponent<EnemyBehhaviour>() != null) monster.GetComponent<EnemyBehhaviour>().enabled = true;
            if (monster.GetComponent<AIMove>() != null) monster.GetComponent<AIMove>().enabled = true;
            if (monster.GetComponent<Monster>() != null) monster.GetComponent<Monster>().TakeDamage(99999);
        }
    }

}
