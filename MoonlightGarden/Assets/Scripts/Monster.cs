using System.Threading;
using UnityEngine;

public class Monster : Character
{
    [SerializeField] protected MonsterType monsterType;
    private ItemDropper itemDropper;

    public Animator animator;
    Rigidbody2D rb2D;
    public Transform target;
    Vector2 Direction;
    public void UpdateDirection()
    {
        Direction = target.position - transform.position;
        if(Direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    protected enum MonsterType
    {
        None, Doggo, B
    }

    public override void Start()
    {
        base.Start(); 
        itemDropper = GetComponent<ItemDropper>();
        rb2D = GetComponent<Rigidbody2D>();
        GetComponent<AIMove>().speed = moveSpeed;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage); 
        StartCoroutine( DisplayTakeDamage());
        GetComponent<EnemyBehhaviour>().ChangeState(EnemyBehhaviour.EnemyState.TakeDamage);
        //เรียก rigi knockback?
        if (currentHealth <= 0)
        {
            Debug.Log($"Monster {monsterType} died.");
            if (itemDropper != null)
            {
                itemDropper.TryDropItem(); // เรียกใช้ TryDropItem เมื่อมอนสเตอร์ตาย
            }
            SummonTower summonTower = GetComponentInParent<SummonTower>();
            if (summonTower != null)
            {
                summonTower.ReturnToPool(gameObject);
            }
            else
            {
                gameObject.SetActive(false); // ถ้าไม่มี SummonTower ให้ปิดใช้งาน GameObject
                GameManager.instance.enemyOverAllControl.monsterInScene.Remove(transform.gameObject);
            }
            currentHealth = MaxHealth;

            this.gameObject.SetActive(false);
            this.currentHealth = MaxHealth;
            spriteRenderer.color = originalColor;
            GetComponent<EnemyBehhaviour>().ChangeState(EnemyBehhaviour.EnemyState.Die);
            GameManager.instance.sessionData.totalKill += 1;
            GameManager.instance.flowerTransform.GetComponent<MoonFlower>().UpdateNutrient(10);
            Debug.LogError($"{name} died!!!!!!!");
        }
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
    }
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;
    private float nextAttackTime = 0f;

    public AudioClip attackSound;

    public float knockBackPower = 0.6f;
    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            //Debug.Log($"Monster {monsterType} attacks!");
            GameManager.instance.soundManager.PlayOneShotWithVaryPitch(GameManager.instance.soundManager.monsterSource, attackSound);
           
            GameManager.instance.playerController.TakeDamage(attackDamage);
            GameManager.instance.playerCharacter.GetKnockBack(knockBackPower, transform.position - GameManager.instance.playerCharacter.transform.position);
            nextAttackTime = Time.time + attackCooldown;
        }
    }
    public void AttackBase()
    {
        if (Time.time >= nextAttackTime)
        {
            GameManager.instance.soundManager.PlayOneShotWithVaryPitch(GameManager.instance.soundManager.monsterSource, attackSound);
            GameManager.instance.flowerTransform.GetComponent<MoonFlower>().TakeDamage();
            nextAttackTime = Time.time + attackCooldown;
        }
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void GetKnockBack(float power)
    {
        rb2D.AddForce(-Direction.normalized * power,ForceMode2D.Impulse);
    }
}