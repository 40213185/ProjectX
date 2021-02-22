using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : Item
{
    public enum UsableType { Potion, Bomb }
    private UsableType type;
    private StatusEffect.EffectType effectType = StatusEffect.EffectType.None;


    public Usable(string name, string description, Vector2 range)
    {
        int random;
        switch (type)
        {
            case UsableType.Potion:
                random = Random.Range(0, 3);
                switch (random)
                {
                    case 0:
                        effectType = StatusEffect.EffectType.Healing;
                        break;
                    case 1:
                        effectType = StatusEffect.EffectType.StrengthBuff;
                        break;
                    case 2:
                        effectType = StatusEffect.EffectType.IntBuff;
                        break;
                }

                switch (effectType)
                {
                    case StatusEffect.EffectType.Healing:
                        SetNameAndDesc("Healing Potion", "Heals you for a portion of your health, No idea how, isnt modern medicine amazing");
                        break;
                    case StatusEffect.EffectType.StrengthBuff:
                        SetNameAndDesc("Strength Potion", "Looks like milk, doesnt taste like milk, but still makes you big and strong so who cares");
                        break;
                    case StatusEffect.EffectType.IntBuff:
                        SetNameAndDesc("Intelligence Potion", "Its big brain juice, has the opposite affect of alcohol and less fun");
                        break;
                }
                break;
            case UsableType.Bomb:
                random = Random.Range(0, 4);
                switch (random)
                {
                    case 0:
                        effectType = StatusEffect.EffectType.Bleed;
                        break;
                    case 1:
                        effectType = StatusEffect.EffectType.Burn;
                        break;
                    case 2:
                        effectType = StatusEffect.EffectType.Freeze;
                        break;
                    case 3:
                        effectType = StatusEffect.EffectType.Poisoned;
                        break;
                }

                switch (effectType)
                {
                    case StatusEffect.EffectType.Bleed:
                        SetNameAndDesc("Spikey Bomb", "A bomb full of spikey objects, Cover your eyes kiddies");
                        break;
                    case StatusEffect.EffectType.Burn:
                        SetNameAndDesc("Fire Bomb", "A bomb full of fire, Common tool amoung chefs");
                        break;
                    case StatusEffect.EffectType.Freeze:
                        SetNameAndDesc("Ice Bomb", "A very cold bomb, Can make a snowball fight deadly");
                        break;
                    case StatusEffect.EffectType.Poisoned:
                        SetNameAndDesc("Poison Bomb", "A Poison bomb, Smells so bad it makes you ill");
                        break;
                }
                break;


        }

    }
    public UsableType getUsableType()
    {
        return type;
    }

    /*public Vector2 getAreaOfEffect(UsableType usableType)
    {
        
    }*/

    public void use()
    {

    }
}