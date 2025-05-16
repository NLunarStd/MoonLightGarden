using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttackMonster : MonoBehaviour
{
    public bool isAttacking = false;
    public int damageToInflicted;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (isAttacking)
            {
                if (collision.CompareTag("Monster"))
                {
                    
                    //Debug.Log($"â¨ÁµÕ {collision.name}");
                    collision.GetComponent<Monster>().TakeDamage(damageToInflicted);
                        
                    
                    
                }
            }
            
        }
    }

}
