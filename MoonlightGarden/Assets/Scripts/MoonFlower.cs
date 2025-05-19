using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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
}
