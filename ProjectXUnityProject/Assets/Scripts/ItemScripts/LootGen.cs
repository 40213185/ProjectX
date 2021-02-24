using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGen : MonoBehaviour
{
    void Start()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        if (Random.Range(0,100) > 75) 
        {
            Random.seed = System.DateTime.Now.Millisecond;
            int rarity = Random.Range(0, 100);
            Weapon weapon = new Weapon(rarity,new Vector2(1, 1), 1);
        }
    }
}
