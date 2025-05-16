using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    public Animator animator;
    public Transform shadow;

    public int plantStage = 1;


    public float growthTimeToStage2;
    public float growthTimeToStage3;
    public float growthTimeToStage4;
    public float growthTimeToStage5;

    public SummonTower summonTower;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(Growing());

        summonTower = GetComponent<SummonTower>();
        summonTower.enabled = false ;
    }
    public void SetPlantStage(int stage)
    {
        plantStage = stage;
        animator.SetInteger("Stage",plantStage);
    }
    void SetShadowScale(float scale)
    {
        shadow.localScale = shadow.localScale * scale;
    }
    IEnumerator Growing()
    {
        SetPlantStage(1);
        yield return new WaitForSeconds(growthTimeToStage2);
        SetPlantStage(2);
        SetShadowScale(3f);
        yield return new WaitForSeconds(growthTimeToStage3);
        SetPlantStage(3);
        SetShadowScale(1.5f);
        yield return new WaitForSeconds(growthTimeToStage4);
        SetPlantStage(4);
        SetShadowScale(1.5f);
        yield return new WaitForSeconds(growthTimeToStage5);
        SetPlantStage(5);
        SetShadowScale(1.5f);
        summonTower.enabled = true ;
    }


}
