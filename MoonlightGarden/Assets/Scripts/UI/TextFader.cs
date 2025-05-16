using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextFader : MonoBehaviour
{
    public float fadeSpeed = 0.01f; 

    private TextMeshProUGUI text;
    private bool isCompleteFade = false;

    private void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        isCompleteFade = false;
        StartCoroutine(FadeOutText());
    }

    IEnumerator FadeOutText()
    {
        if (!isCompleteFade)
        {
            while (text.color.a > 0)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - fadeSpeed);
                yield return null; 
            }
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        this.transform.parent.gameObject.SetActive(false);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        isCompleteFade = true;
    }
}