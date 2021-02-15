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
    private StatusEffect.EffectType effectType;

    public Weapon(string name, string description, int rarity, Vector2 range, int floor)
    {
        type = (EquipmentType)Random.Range(0, 5);
        switch (type)
        {
            case EquipmentType.Dagger:
                Debug.Log("Dagger");
                type = EquipmentType.Dagger;
                skill = skills.Stab;
                SetPotency(1, 3, floor, rarity);
                break;

            case EquipmentType.Sword:
                Debug.Log("Sword");
                type = EquipmentType.Sword;
                skill = skills.Slash;
                SetPotency(2, 5, floor, rarity);
                break;

            case EquipmentType.TwoHandedSword:
                Debug.Log("2HSword");
                type = EquipmentType.TwoHandedSword;
                skill = skills.Smash;
                SetPotency(4, 8, floor, rarity);
                break;

            case EquipmentType.FireBall:
                Debug.Log("FireBall");
                type = EquipmentType.FireBall;
                skill = skills.Throw;
                SetPotency(3, 6, floor, rarity);
                break;

            case EquipmentType.IceSpike:
                Debug.Log("IceSpike");
                type = EquipmentType.IceSpike;
                skill = skills.Throw;
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
