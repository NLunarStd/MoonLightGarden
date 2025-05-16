using UnityEngine;

public class MonsterCorpse : Item
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        base.UseItem();
    }
}