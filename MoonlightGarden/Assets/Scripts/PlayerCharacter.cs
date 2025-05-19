using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    PlayerController playerController;
    public bool isInCombatState = false;
    public int combatStateDuration = 3;
    public override void Start()
    {
        base.Start();
        isPlayerDead = false;
        characterName = GameManager.instance.playerName;
        playerController = GameManager.instance.playerController;
    }
    public bool isPlayerDead = false;
    public override void TakeDamage(int damage)
    {
        float remainingPercentage = (float)currentHealth / (float)MaxHealth;
        
        
        isInCombatState = true;
        if (!isStartExitCombat)
        {
            StartCoroutine(ExitingCombatState());
        }
        if (isPlayerDead)
        {
            return;
        }
        base.TakeDamage(damage);
        
        GameManager.instance.uIController.UpdateHP(remainingPercentage);
        StartCoroutine(DisplayTakeDamage()); 
        if (currentHealth <= 0)
        {
            isPlayerDead = true;
            SetToDeadState();
            RespawnCooldown();
        }
    }
    bool isStartExitCombat = false;
    IEnumerator ExitingCombatState()
    {
        isStartExitCombat = true;
        if (isInCombatState)
        {
            yield return new WaitForSeconds(3);
            isInCombatState = false;
        }
        yield return null;
        isStartExitCombat = false;
    }
    bool isNaturalHealingStarted = false;
    IEnumerator NaturalHealing()
    {
        isNaturalHealingStarted = true;
        while (!isInCombatState)
        {
            yield return new WaitForSeconds(3);
            Heal(1);
        }
        isNaturalHealingStarted = false;
        yield return null;
    }
    private void Update()
    {
        if (!isNaturalHealingStarted && !isInCombatState)
        {
            StartCoroutine(NaturalHealing());
        }
    }
    public override void Heal(int amount)
    {
        base.Heal(amount);
        GameManager.instance.uIController.UpdateHP((float)currentHealth / (float)MaxHealth);
    }

    public void RespawnCooldown()
    {
        
        GameManager.instance.uIController.UpdateRespawningCooldown("3");
        GameManager.instance.uIController.ToggleRespawningScreen();
        StartCoroutine(Respawning());
    }

    IEnumerator Respawning()
    {
        yield return new WaitForSeconds(1);
        GameManager.instance.uIController.UpdateRespawningCooldown("2");
        yield return new WaitForSeconds(1);
        GameManager.instance.uIController.UpdateRespawningCooldown("1");
        yield return new WaitForSeconds(1);
        GameManager.instance.uIController.UpdateRespawningCooldown("0");
        yield return null;
        GameManager.instance.uIController.ToggleRespawningScreen();
        transform.position = GameManager.instance.respawnPoint.position;
        currentHealth = maxHealth;
        GameManager.instance.uIController.UpdateHP((float)currentHealth / (float)MaxHealth);
        isPlayerDead = false;
        SetToDeadState();
    }

    public void SetToDeadState()
    {
        if (isPlayerDead)
        {
            this.enabled = false;
            transform.GetComponent<Collider2D>().enabled = false;
            transform.GetComponent<PlayerController>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);

        }
        else
        {
            this.enabled = true;
            transform.GetComponent<Collider2D>().enabled = true;
            transform.GetComponent<PlayerController>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }

    }

    public void GetKnockBack(float power,Vector2 direction)
    {
         playerController.rb2D.AddForce(direction.normalized * power, ForceMode2D.Impulse);
    }
}
