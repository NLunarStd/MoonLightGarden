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
        triggerAttackMonster.damageToInflicted = damage;
        collider2D = GetComponent<Collider2D>();
    }

    public virtual void Attack()
    {
        EnableHitbox();
        GameManager.instance.soundManager.playerSource.PlayOneShot(attackSound);
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