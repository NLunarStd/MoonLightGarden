using UnityEngine;

public class Equipment : Item
{
    public string equipmentName; 

    public bool isWeapon;
    public Weapon weapon;

    private void Start()
    {
        if(isWeapon)
        {

            weapon = GetComponentInChildren<Weapon>();
        }
    }

    public override void UseItem()
    {
        base.UseItem();
       // Debug.Log("Use " + equipmentName); 
        gameObject?.SetActive(true);
        
        PlayerController.instance.weaponSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if(isWeapon)
        {
            weapon.animator.SetTrigger("Attack");
            weapon?.Attack();
        }
    }
    
    
    public void DeactivateEquipment()
    {
        gameObject.SetActive(false);
    }
}