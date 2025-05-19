using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 10;
    public float attackRange = 1f;
    public Animator animator;
    public bool isAttacking = false;

    public int maxTargets = 3; 
    public float attackDuration = 1f;
    TriggerAttackMonster triggerAttackMonster;

    public Collider2D collider2D;

    public AudioClip attackSound;

    private void Start()
    {
        animator = GetComponent<Animator>();
        triggerAttackMonster = GetComponent<TriggerAttackMonster>();
        int v = damage + GameManager.instance.playerCharacter.Attack;
        int finalDamage = v;
        triggerAttackMonster.damageToInflicted = finalDamage;
        collider2D = GetComponent<Collider2D>();
    }

    public virtual void Attack()
    {
        EnableHitbox();
        GameManager.instance.soundManager.PlayOneShotWithVaryPitch(GameManager.instance.soundManager.playerSource, attackSound);
        triggerAttackMonster.isAttacking = true;
    }

    public void EnableHitbox()
    {
        collider2D.enabled = true;
    }

    public void DisableHitbox()
    {
        collider2D.enabled = false;
    }
}