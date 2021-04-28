using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : Item
{
    public enum UsableType { Potion, Bomb }
    private UsableType type;
    private StatusEffect.EffectType effectType = StatusEffect.EffectType.None;
    private GameObject player;

    public Usable(UsableType usabletype) 
    {
        type = usabletype;
        RollUsableEffect(type);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public UsableType GetUsableType()
    {
        return type;
    }
    public StatusEffect.EffectType GetEffectType()
    {
        return effectType;
    }

    //used for adding the effect components or
    //dealing dmg/healing
    public void Use(GameObject[] entitiesAffected)
    {
        foreach (GameObject ent in entitiesAffected)
        {
            switch (type)
            {
                case UsableType.Potion: 
                    {
                        switch (effectType)
                        {
                            case StatusEffect.EffectType.Healing:
                                {
                                    if (ent.tag == "Player") ent.GetComponent<PlayerController>().stats.ModifyHealthBy(RollForPotency());
                                    else if (ent.tag == "Enemy") ent.GetComponent<EnemyController>().GetStats().ModifyHealthBy(RollForPotency());
                                    break;
                                }
                            case StatusEffect.EffectType.StrengthBuff:
                                {
                                    var component = ent.AddComponent<StatusEffect>();
                                    component.setStatusEffect(effectType, 3, RollForPotency(),player);
                                    break;
                                }
                            case StatusEffect.EffectType.IntBuff:
                                {
                                    var component = ent.AddComponent<StatusEffect>();
                                    component.setStatusEffect(effectType, 3, RollForPotency(), player);
                                    break;
                                }
                        }
                        break;
                    }
                case UsableType.Bomb:
                    {
                        switch (effectType)
                        {
                            case StatusEffect.EffectType.Burn:
                                {
                                    var component = ent.AddComponent<StatusEffect>();
                                    component.setStatusEffect(effectType, Random.Range(2,6), RollForPotency(), player);
                                    break;
                                }
                            case StatusEffect.EffectType.Freeze:
                                {
                                    var component = ent.AddComponent<StatusEffect>();
                                    component.setStatusEffect(effectType, 1, RollForPotency(), player);
                                    break;
                                }
                            case StatusEffect.EffectType.Poisoned:
                                {
                                    var component = ent.AddComponent<StatusEffect>();
                                    component.setStatusEffect(effectType, 4, RollForPotency(), player);
                                    break;
                                }
                            case StatusEffect.EffectType.Bleed:
                                {
                                    var component = ent.AddComponent<StatusEffect>();
                                    component.setStatusEffect(effectType, 3,RollForPotency(), player);
                                    break;
                                }
                        }
                        break;
                    }
            }
        }
    }

    private void RollUsableEffect(UsableType usableType)
    {
        //random int for defining the effecttype associated with this usable
        int random;
        switch (usableType)
        {
            case UsableType.Potion:
                //roll
                random = Random.Range(0, 3);
                //add effect type according to the roll
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
                break;
            case UsableType.Bomb:
                //roll
                random = Random.Range(0, 4);
                //add effect type according to roll
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

                break;
        }

        //set appropriate name and description,range and area of effect and potency
        switch (effectType)
        {
            case StatusEffect.EffectType.Healing:
                SetNameAndDesc("Healing Potion", "Heals you for a portion of your health,\n No idea how, isnt modern medicine amazing");
                SetRangeAndAoE(new Vector2(0,0),new Vector2(0,0));
                SetPotency(3,6,GameData.CurrentFloor,1);
                break;
            case StatusEffect.EffectType.StrengthBuff:
                SetNameAndDesc("Strength Potion", "Looks like milk, doesnt taste like milk,\n but still makes you big and strong so who cares");
                SetRangeAndAoE(new Vector2(0, 0), new Vector2(0, 0));
                SetPotency(2, 4, GameData.CurrentFloor, 1);
                break;
            case StatusEffect.EffectType.IntBuff:
                SetNameAndDesc("Intelligence Potion", "Its big brain juice, has the\n opposite affect of alcohol and less fun");
                SetRangeAndAoE(new Vector2(0, 0), new Vector2(0, 0));
                SetPotency(3, 6, GameData.CurrentFloor, 1);
                break;
            case StatusEffect.EffectType.Bleed:
                SetNameAndDesc("Spikey Bomb", "A bomb full of spikey objects,\n Cover your eyes kiddies");
                SetRangeAndAoE(new Vector2(1, 5), new Vector2(0, 0));
                SetPotency(1, 4, GameData.CurrentFloor, 1);
                break;
            case StatusEffect.EffectType.Burn:
                SetNameAndDesc("Fire Bomb", "A bomb full of fire,\n Common tool amoung chefs");
                SetRangeAndAoE(new Vector2(1, 5), new Vector2(0, 2));
                SetPotency(2, 3, GameData.CurrentFloor, 1);
                break;
            case StatusEffect.EffectType.Freeze:
                SetNameAndDesc("Ice Bomb", "A very cold bomb, Can make\n a snowball fight deadly");
                SetRangeAndAoE(new Vector2(0, 0), new Vector2(0, 3));
                SetPotency(1, 2, GameData.CurrentFloor, 1);
                break;
            case StatusEffect.EffectType.Poisoned:
                SetNameAndDesc("Poison Bomb", "A Poison bomb, Smells so bad\n it makes you ill");
                SetRangeAndAoE(new Vector2(0, 0), new Vector2(0, 4));
                SetPotency(3, 3, GameData.CurrentFloor, 1);
                break;
        }
    }
}