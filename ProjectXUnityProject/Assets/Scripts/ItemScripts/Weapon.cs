using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public enum EquipmentType {Dagger, Sword, TwoHandedSword, FireBall, IceSpike}
    public enum skills {Stab, Slash, Smash, Throw}
    private int rarity;
    private EquipmentType type;
    private skills skill;
    private StatusEffect.EffectType effectType = StatusEffect.EffectType.None;

    public Weapon(string name, string description, int rarity, Vector2 range, int floor)
    {
        type = (EquipmentType)Random.Range(0, 6);
        switch (type)
        {
            case EquipmentType.Dagger:
                Debug.Log("Dagger");
                type = EquipmentType.Dagger;
                skill = skills.Stab;
                effectType = StatusEffect.EffectType.Bleed;
                SetNameAndDesc("Dagger", "A Small bladed dagger, used for stabbing things, Good at making holes that bleed");
                SetPotency(1, 3, floor, rarity);
                break;

            case EquipmentType.Sword:
                Debug.Log("Sword");
                type = EquipmentType.Sword;
                skill = skills.Slash;
                SetNameAndDesc("Sword", "A long bladed weapon, Makes a nice whoosh sound when used in a slashing motion");
                SetPotency(2, 5, floor, rarity);
                break;

            case EquipmentType.TwoHandedSword:
                Debug.Log("2HSword");
                type = EquipmentType.TwoHandedSword;
                skill = skills.Smash;
                effectType = StatusEffect.EffectType.Stun;
                SetNameAndDesc("TwoHandedSword", "A Massive sword, Surprised you can even hold it up, Good at cracking eggs");
                SetPotency(4, 8, floor, rarity);
                break;

            case EquipmentType.FireBall:
                Debug.Log("FireBall");
                type = EquipmentType.FireBall;
                skill = skills.Throw;
                effectType = StatusEffect.EffectType.Burn;
                SetNameAndDesc("Fireball", "Summon a fireball from god knows where and use it for things such as cooking or other activities");
                SetPotency(3, 6, floor, rarity);
                break;

            case EquipmentType.IceSpike:
                Debug.Log("IceSpike");
                type = EquipmentType.IceSpike;
                skill = skills.Throw;
                effectType = StatusEffect.EffectType.Freeze;
                SetNameAndDesc("Icespike", "Summon a giant ice spike that can be launched like a rocket, remember to wear gloves when using this, can get chilly");
                SetPotency(3, 6, floor, rarity);
                break;
        }
    }
    
    public int getRarity()
    {
        return rarity;
    }

    public int RollForDamage()
    {

        Vector2Int damageVector = GetPotency();

        int damage = Random.Range(damageVector.x, damageVector.y);

        if (Random.Range(0, 100) < 15) damage = 0;

        return damage;
    }

    public EquipmentType GetEquipmentType()
    {
        return type;
    }

    public Vector2 getAreaOfEffect(EquipmentType equipmentType)
    {
        return GetAreaOfEffect();
    }

    public void use()
    {
        
    }
}
