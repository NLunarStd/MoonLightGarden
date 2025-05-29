using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class BloodMoonController : MonoBehaviour
{
    bool isStartBloodMoon = false;
    public SummonTower summonTower;
    public WhiteBalance whiteBalance;
    public float tintValue = 100;
    public Volume m_Volume;
    
    void Start()
    {
        summonTower = GetComponent<SummonTower>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckCondition();
    }

    void CheckCondition()
    {
        if(!GameManager.instance.lightCycleController.isNightTime)
        {
            return;
        }
        VolumeProfile profile = m_Volume.sharedProfile;
        if (!profile.TryGet<WhiteBalance>(out var wb))
        {
            wb = profile.Add<WhiteBalance>(false);
        }

        if (GameManager.instance.currentDay % 5 == 0)
        {
            isStartBloodMoon = true;
            wb.tint.Override(tintValue);
            wb.temperature.Override(tintValue);

            if (isStartBloodMoon)
            {
                summonTower.enabled = true;
                summonTower.StartSummon();
            }
            else
            {
                summonTower.enabled = false;
                summonTower.StopSummon();
                foreach (Transform monster in gameObject.transform)
                {
                    summonTower.ReturnToPool(monster.gameObject);
                    monster.localPosition = Vector3.zero;
                }
            }
        }
        else
        {
            isStartBloodMoon = true;
            wb.tint.Override(0);
            wb.temperature.Override(0);
        }

        
    }
}
