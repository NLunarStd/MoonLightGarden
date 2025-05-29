using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SummonTower : Structure
{
    [Header("Summon Tower Parameters")]
    public GameObject monsterPrefab;
    public Transform summonPoint;
    public float summonRate = 5f;
    public int maxMonsters = 5;
    public float summonRadius = 2f;

    [SerializeField]
    private int currentMonsters = 0;
    private List<GameObject> monsterPool = new List<GameObject>(); 

    protected override void OnStructureBuilt()
    {
        base.OnStructureBuilt();
        CreatePool(); 
        
    }
    bool isCoroutineStarted = false;
    public void StartSummon()
    {
        if (isCoroutineStarted) return;
        StartCoroutine(SummonMonsterRoutine());
        isCoroutineStarted = true;
    }
    public void StopSummon()
    {
        StopCoroutine(SummonMonsterRoutine());
        isCoroutineStarted = false;
    }

    private void CreatePool()
    {
        for (int i = 0; i < maxMonsters; i++)
        {
            GameObject monster = Instantiate(monsterPrefab, transform);
            monster.SetActive(false);
            monsterPool.Add(monster);
        }
    }

    private IEnumerator SummonMonsterRoutine()
    {
        while (true) 
        {
            if (CountActiveMonsters() < maxMonsters) 
            {
                SummonMonster();
                yield return new WaitForSeconds(1f / summonRate);
            }
            else
            {
                yield return null; 
            }
        }
    }
    private int CountActiveMonsters()
    {
        int activeMonsters = 0;
        foreach (GameObject monster in monsterPool)
        {
            if (monster.activeInHierarchy)
            {
                activeMonsters++;
            }
        }
        return activeMonsters;
    }
    private void SummonMonster()
    {
        Vector3 randomPosition = Random.insideUnitSphere * summonRadius;
        randomPosition.y = 0;
        Vector3 spawnPosition = summonPoint.position + randomPosition;

        GameObject monster = GetMonsterFromPool();
        if (monster != null)
        {
            monster.transform.position = spawnPosition;
            monster.SetActive(true); 
            GameManager.instance.enemyOverAllControl.monsterInScene.Add(monster);
        }
    }

    private GameObject GetMonsterFromPool()
    {
        foreach (GameObject monster in monsterPool)
        {
            if (!monster.activeInHierarchy)
            {
                return monster; 
            }
        }
        return null; 
    }

    public void ReturnToPool(GameObject monster)
    {
        monster.SetActive(false);
        currentMonsters -= 1;
        GameManager.instance.enemyOverAllControl.monsterInScene.Remove(monster);
    }

    protected override void DestroyStructure()
    {
        base.DestroyStructure();
        StopAllCoroutines();
        foreach (GameObject monster in monsterPool)
        {
            if (monster.activeInHierarchy)
            {
                ReturnToPool(monster); 
            }
        }
    }
}