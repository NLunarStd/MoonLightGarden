using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightCycleController : MonoBehaviour
{
    public Gradient gradient;
    public Light2D globalLight2D;

    public Color dayColor;
    public Color nightColor;
    public Color twilightColor;
    public float dayLightIntensity;
    public float twilightLightIntensity;
    public float nightLightIntensity;
    float currentLightIntensity;


    public float dayNightCycleDuration = 480f; 
    public float stayDayorNightDuration = 120;
    private float currentTime = 0f;
    private bool isDayToNight = true; 
    private bool isWaiting = false; 
    public bool isNightTime = false;
    

    
    private void Start()
    {
        SetUPColor();
        UpdateLightColor();
        
    }
    private void Update()
    {
        if (isWaiting) return; 

        currentTime += Time.deltaTime;
        //Debug.Log(currentTime);
        if (currentTime <= dayNightCycleDuration)
        {
            UpdateLightColor();
            UpdateLightIntensity();
        }
        else if (currentTime > dayNightCycleDuration)
        {
            currentTime = dayNightCycleDuration;
            isDayToNight = !isDayToNight;
            StartCoroutine(WaitAndReverse());
        }
        if (currentTime <= 0)
        {
            currentTime = 0;
            isDayToNight = !isDayToNight;
            StartCoroutine(WaitAndReverse());
        }
        
    }
    void SetUPColor()
    {
        GradientColorKey[] colorKeys = new GradientColorKey[3];
        colorKeys[0] = new GradientColorKey(dayColor, 0.3f);
        colorKeys[1] = new GradientColorKey(twilightColor, 0.65f);
        colorKeys[2] = new GradientColorKey(nightColor, 0.85f);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[3];
        alphaKeys[0] = new GradientAlphaKey(1f, 1f);
        alphaKeys[1] = new GradientAlphaKey(1f, 1f);
        alphaKeys[2] = new GradientAlphaKey(1f, 1f);
        gradient.SetKeys(colorKeys, alphaKeys);


        currentLightIntensity = globalLight2D.intensity;

    }

    public float normalizedTime;
    void UpdateLightIntensity()
    {
        float targetIntensity;
        if (normalizedTime <= 0.3f)
        {
            targetIntensity = dayLightIntensity;
            isNightTime = false;
        }
        else if (normalizedTime <= 0.65f)
        {
            targetIntensity = twilightLightIntensity;
            isNightTime = false;
        }
        else
        {
            targetIntensity = nightLightIntensity;
            isNightTime = true;
        }

        if (currentLightIntensity != targetIntensity)
        {
            StartCoroutine(TransitionIntensity(targetIntensity));
        }

    }
    IEnumerator TransitionIntensity(float target)
    {
        float startTime = Time.time;
        float duration = 10f;


        while (Mathf.Abs(currentLightIntensity - target) > 0.01f)
        {
            float timePassed = Time.time - startTime;
            float lerpTime = Mathf.Clamp01(timePassed / duration);
            currentLightIntensity = Mathf.Lerp(currentLightIntensity, target, lerpTime);
            globalLight2D.intensity = currentLightIntensity;

            yield return null;
        }
        currentLightIntensity = target;
        globalLight2D.intensity = currentLightIntensity;
    }
    void UpdateLightColor()
    {
        if (isDayToNight)
        {
            normalizedTime = currentTime / dayNightCycleDuration;
        }
        else
        {
            normalizedTime = 1f - (currentTime / dayNightCycleDuration); 
        }
        if(normalizedTime >= 1)
        {
            isDayToNight = true;
        }
        Color targetColor = gradient.Evaluate(normalizedTime);
        globalLight2D.color = targetColor;
    }
    IEnumerator WaitAndReverse()
    {
        isWaiting = true;
        yield return new WaitForSeconds(stayDayorNightDuration);
        isWaiting = false;
        if (!isDayToNight)
        {
            GameManager.instance.UpdateDay();
        }
        currentTime = 0f;

    }

}
