using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoonFlower : Interactable
{
    public int health = 10;
    public int currentHealth;
    public Transform craftingPanel;
    public SpriteRenderer spriteRenderer;
    public Color originalColor = Color.white;
    public ParticleSystem hitParticle;
    private void Start()
    {
        currentHealth = health;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartSeedCycle();
    }
    public override void UseItem(PlayerController player)
    {
        base.UseItem(player);

    }
    public void ResetHealth()
    {
        currentHealth = health;
        float remainingPercentage = (float)currentHealth / (float)health;
        GameManager.instance.uIController.UpdateFlowerHP(remainingPercentage);
        GameManager.instance.enemyOverAllControl.DestroyAllMonsterInScene();
    }
    public void TakeDamage()
    {
        currentHealth -= 1;
        if (currentHealth > 4)
        {
            GameManager.instance.uIController.flowerAlertText.text = "Flower is being Attack!";
        }
        else 
        {
            GameManager.instance.uIController.flowerAlertText.text = "Flower is about to Wither!";
        }
        GameManager.instance.uIController.DisplayAlertText();
        float remainingPercentage = (float)currentHealth / (float)health;
        GameManager.instance.uIController.UpdateFlowerHP(remainingPercentage);
        StartCoroutine(DisplayTakeDamage());
        hitParticle.Play();
        if (currentHealth <= 0)
        {
            GameManager.instance.UpdateGameState(GameManager.GameState.GameOver);
        }
    }
    public IEnumerator DisplayTakeDamage()
    {

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }

    }
    public void SetToDeadState()
    {
        if (GameManager.instance.isGameOver)
        {
            this.enabled = false;
            transform.GetComponent<Collider2D>().enabled = false;

        }
        else
        {
            this.enabled = true;
            transform.GetComponent<Collider2D>().enabled = true;
        }

    }

    public int nutrientValue = 100;
    public int seedTime = 180;
    public TextMeshProUGUI nutrientText;
    public TextMeshProUGUI seedTimeText;
    float timer = 180;
    public bool isAbleToGrantSeed = false;
    public Transform grantSeedPosition;
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0;
            }
            DisplayTime(timer);
        }
        else
        {
            if(nutrientValue > 20)
            {
                isAbleToGrantSeed = true;
                // send seed
                if (isAbleToGrantSeed)
                {
                    AbsorbNutrient(-20);
                    isAbleToGrantSeed = false ;
                }
            }
            StartSeedCycle();
        }
    }
    public void StartSeedCycle()
    {
        timer = seedTime;
    }
    public void AbsorbNutrient(int value)
    {
        UpdateNutrient(value);
        if(nutrientValue <= 0)
        {
            TakeDamage();
        }
    }
    public void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        seedTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void UpdateNutrient(int value)
    {
        nutrientValue += value;
        nutrientText.text = $"Nutrient : {nutrientValue}";
    }
}
