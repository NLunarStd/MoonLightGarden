using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void UseItem(PlayerController player)
    {
        //Debug.Log(this.name + " use by " +  player.name);
    }
}
