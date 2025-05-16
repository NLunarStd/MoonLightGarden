using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : Interactable
{
    bool isPicked = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if(collision.CompareTag("Player") && !isPicked)
            {
               // Debug.LogWarning("pick");
                isPicked = true;
                transform.parent = PlayerController.instance.handPoint;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                this.gameObject.SetActive(false);

                GameManager.instance.AddItem(this.GetComponent<Item>(), 1);
                this.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
    public override void UseItem(PlayerController player)
    {
        base.UseItem(player);
    }

}
