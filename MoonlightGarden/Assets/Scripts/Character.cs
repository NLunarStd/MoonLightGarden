using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected string characterName;
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int attack = 10;
    [SerializeField] protected int armor = 0;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float rotationSpeed = 100f;
    [SerializeField] protected GameObject prefabs;

    public string CharacterName => characterName;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public int Attack => attack;
    public int Armor => armor;
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public GameObject Prefabs => prefabs;
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor;
    public virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = damage - armor;
        if (actualDamage < 0) actualDamage = 0;
        currentHealth -= actualDamage;
        if (currentHealth < 0) currentHealth = 0;
        //Debug.Log($"Took {actualDamage} damage. Current health: {currentHealth}");
    }

    public virtual void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        //Debug.Log($"Healed {amount}. Current health: {currentHealth}");
    }

    //ItemDropper itemDropper;

    public IEnumerator DisplayTakeDamage()
    {
        
        if(spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }
        
    }
}