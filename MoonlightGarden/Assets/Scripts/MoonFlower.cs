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
    }
    public void TakeDamage()
    {
        currentHealth -= 1;
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
}
