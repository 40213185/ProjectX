using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : Item
{
    public enum UsableType {Potion, SplashPotion, Scroll, Bomb}
    private UsableType type;

    public UsableType getUsableType()
    {
        return type;
    }

    /*public Vector2 GetAreaOfEffect(UsableType usableType)
    {
        
    }*/

    public void use()
    {

    }
}
