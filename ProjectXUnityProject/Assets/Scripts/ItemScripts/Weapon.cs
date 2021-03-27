using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public enum EquipmentType { Dagger, Sword, TwoHandedSword, FireBall, IceSpike }
    public enum Skills { Stab, Slash, Smash, Throw }
    private int rarity;
    private EquipmentType type;
    private Skills skill;
    private StatusEffect.EffectType effect;
    private int cost;

    public static Weapon GetRandomWeapon(int modifier1, int modifier2)
    {
        Weapon weaponRoll = new Weapon();
        int rndType = Random.Range(0, 5);
        EquipmentType equipType=EquipmentType.Dagger;
        switch (rndType)
        {
            case 0: { equipType = EquipmentType.Dagger; break; }
            case 1: { equipType = EquipmentType.Sword; break; }
            case 2: { equipType = EquipmentType.TwoHandedSword; break; }
            case 3: { equipType = EquipmentType.FireBall; break; }
            case 4: { equipType = EquipmentType.IceSpike; break; }
        }
        switch (equipType)
        {
            case EquipmentType.Dagger:
                weaponRoll.type = EquipmentType.Dagger;
                weaponRoll.skill = Skills.Stab;
                weaponRoll.effect = StatusEffect.EffectType.Bleed;
                weaponRoll.SetNameAndDesc("Dagger", "A Small bladed dagger, used for stabbing things, Good at making holes that bleed");
                weaponRoll.SetPotency(1, 3, modifier2, modifier1);
                weaponRoll.SetRangeAndAOEforSkill(weaponRoll.skill);
                weaponRoll.cost = 1;
                break;

            case EquipmentType.Sword:
                weaponRoll.type = EquipmentType.Sword;
                weaponRoll.skill = Skills.Slash;
                weaponRoll.effect = StatusEffect.EffectType.None;
                weaponRoll.SetNameAndDesc("Sword", "A long bladed weapon, Makes a nice whoosh sound when used in a slashing motion");
                weaponRoll.SetPotency(2, 5, modifier2, modifier1);
                weaponRoll.SetRangeAndAOEforSkill(weaponRoll.skill);
                weaponRoll.cost = 1;
                break;

            case EquipmentType.TwoHandedSword:
                weaponRoll.type = EquipmentType.TwoHandedSword;
                weaponRoll.skill = Skills.Smash;
                weaponRoll.effect = StatusEffect.EffectType.Stun;
                weaponRoll.SetNameAndDesc("TwoHandedSword", "A Massive sword, Surprised you can even hold it up, Good at cracking eggs");
                weaponRoll.SetPotency(4, 8, modifier2, modifier1);
                weaponRoll.SetRangeAndAOEforSkill(weaponRoll.skill);
                weaponRoll.cost = 2;
                break;

            case EquipmentType.FireBall:
                weaponRoll.type = EquipmentType.FireBall;
                weaponRoll.skill = Skills.Throw;
                weaponRoll.effect = StatusEffect.EffectType.Burn;
                weaponRoll.SetNameAndDesc("Fireball", "Summon a fireball from god knows where and use it for things such as cooking or other activities");
                weaponRoll.SetPotency(3, 6, modifier2, modifier1);
                weaponRoll.SetRangeAndAOEforSkill(weaponRoll.skill);
                weaponRoll.cost = 3;
                break;

            case EquipmentType.IceSpike:
                weaponRoll.type = EquipmentType.IceSpike;
                weaponRoll.skill = Skills.Throw;
                weaponRoll.effect = StatusEffect.EffectType.Freeze;
                weaponRoll.SetNameAndDesc("Icespike", "Summon a giant ice spike that can be launched like a rocket, remember to wear gloves when using this, can get chilly");
                weaponRoll.SetPotency(3, 6, modifier2, modifier1);
                weaponRoll.SetRangeAndAOEforSkill(weaponRoll.skill);
                weaponRoll.cost = 3;
                break;
        }

        return weaponRoll;
    }
    public void CreateWeapon(EquipmentType equipType, Skills equipSkill,int minPotency,int maxPotency,int modifier1,int modifier2)
    {
        type = equipType;
        skill = equipSkill;
        effect = StatusEffect.EffectType.None;
        SetRangeAndAOEforSkill(skill); 
        SetPotency(minPotency, maxPotency, modifier2, modifier1);
    }
    public void CreateWeapon(EquipmentType equipType, Skills equipSkill, StatusEffect.EffectType effectType, int minPotency, int maxPotency, int modifier1, int modifier2)
    {
        type = equipType;
        skill = equipSkill;
        effect = effectType;
        SetRangeAndAOEforSkill(skill);
        SetPotency(minPotency, maxPotency, modifier2, modifier1);
    }

    private void SetRangeAndAOEforSkill(Skills equipSkill) 
    {
        switch (equipSkill)
        {
            case Skills.Slash:
                {
                    SetRangeAndAoE(new Vector2(1, 1), new Vector2(0, 0));
                    break;
                }
            case Skills.Smash:
                {
                    SetRangeAndAoE(new Vector2(1, 1), new Vector2(0, 0));
                    break;
                }
            case Skills.Stab:
                {
                    SetRangeAndAoE(new Vector2(1, 1), new Vector2(0, 0));
                    break;
                }
            case Skills.Throw:
                {
                    SetRangeAndAoE(new Vector2(3, 4), new Vector2(0, 1));
                    break;
                }
        }
    }

    public int getCost() 
    {
        return cost;
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

    public void use()
    {

    }
}