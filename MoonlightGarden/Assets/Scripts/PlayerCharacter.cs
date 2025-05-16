using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    PlayerController playerController;
    public override void Start()
    {
        base.Start();
        characterName = GameManager.instance.playerName;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        StartCoroutine(DisplayTakeDamage()); 
        if (currentHealth <= 0)
        {
            Debug.Log("Character Dead");

            currentHealth = maxHealth;
            transform.position = GameManager.instance.respawnPoint.position;
        }
    }

    public override void Heal(int amount)
    {
        base.Heal(amount); 
    }
}
