using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public enum EquipmentType { Dagger, Sword, TwoHandedSword, FireBall, IceSpike }
    public enum skills { Stab, Slash, Smash, Throw }
    private int rarity;
    private EquipmentType type;
    private skills skill;
    private StatusEffect.EffectType effectType;

    public Weapon GetRandomWeapon(int rarity, int floor)
    {
        Weapon weaponRoll = new Weapon();
        int weaponType = Random.Range(0, 6);
        switch (type)
        {
            case EquipmentType.Dagger:
                Debug.Log("Dagger");
                weaponRoll.type = EquipmentType.Dagger;
                weaponRoll.skill = skills.Stab;
                weaponRoll.effectType = StatusEffect.EffectType.Bleed;
                SetNameAndDesc("Daggar", "A Small bladed dagger, used for stabbing things, Good at making holes that bleed");
                SetPotency(1, 3, floor, rarity);
                break;

            case EquipmentType.Sword:
                Debug.Log("Sword");
                weaponRoll.type = EquipmentType.Sword;
                weaponRoll.skill = skills.Slash;
                weaponRoll.effectType = StatusEffect.EffectType.None;
                weaponRoll.SetNameAndDesc("Sword", "A long bladed weapon, Makes a nice whoosh sound when used in a slashing motion");
                weaponRoll.SetPotency(2, 5, floor, rarity);
                break;

            case EquipmentType.TwoHandedSword:
                Debug.Log("2HSword");
                weaponRoll.type = EquipmentType.TwoHandedSword;
                weaponRoll.skill = skills.Smash;
                weaponRoll.effectType = StatusEffect.EffectType.Stun;
                weaponRoll.SetNameAndDesc("TwoHandedSword", "A Massive sword, Surprised you can even hold it up, Good at cracking eggs");
                weaponRoll.SetPotency(4, 8, floor, rarity);
                break;

            case EquipmentType.FireBall:
                Debug.Log("FireBall");
                weaponRoll.type = EquipmentType.FireBall;
                weaponRoll.skill = skills.Throw;
                weaponRoll.effectType = StatusEffect.EffectType.Burn;
                weaponRoll.SetNameAndDesc("Fireball", "Summon a fireball from god knows where and use it for things such as cooking or other activities");
                weaponRoll.SetPotency(3, 6, floor, rarity);
                break;

            case EquipmentType.IceSpike:
                Debug.Log("IceSpike");
                weaponRoll.type = EquipmentType.IceSpike;
                weaponRoll.skill = skills.Throw;
                weaponRoll.effectType = StatusEffect.EffectType.Freeze;
                weaponRoll.SetNameAndDesc("Icespike", "Summon a giant ice spike that can be launched like a rocket, remember to wear gloves when using this, can get chilly");
                weaponRoll.SetPotency(3, 6, floor, rarity);
                break;
        }

        return weaponRoll;
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