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
    private bool linearRange;
    private bool linearAOE;

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
    public void CreateWeapon(EquipmentType equipType, Skills equipSkill,int usecost,int minPotency,int maxPotency,int modifier1,int modifier2)
    {
        type = equipType;
        skill = equipSkill;
        effect = StatusEffect.EffectType.None;
        SetRangeAndAOEforSkill(skill); 
        SetPotency(minPotency, maxPotency, modifier2, modifier1);
        cost = usecost;
    }
    public void CreateWeapon(EquipmentType equipType, Skills equipSkill, StatusEffect.EffectType effectType,int usecost, int minPotency, int maxPotency, int modifier1, int modifier2)
    {
        type = equipType;
        skill = equipSkill;
        effect = effectType;
        SetRangeAndAOEforSkill(skill);
        SetPotency(minPotency, maxPotency, modifier2, modifier1);
        cost = usecost;
    }

    private void SetRangeAndAOEforSkill(Skills equipSkill) 
    {
        switch (equipSkill)
        {
            case Skills.Slash:
                {
                    SetRangeAndAoE(new Vector2(1, 1), new Vector2(0, 0));
                    linearRange = true;
                    linearAOE = true;
                    break;
                }
            case Skills.Smash:
                {
                    SetRangeAndAoE(new Vector2(1, 1), new Vector2(0, 0));
                    linearRange = true;
                    linearAOE = true;
                    break;
                }
            case Skills.Stab:
                {
                    SetRangeAndAoE(new Vector2(1, 1), new Vector2(0, 0));
                    linearRange = true;
                    linearAOE = true;
                    break;
                }
            case Skills.Throw:
                {
                    SetRangeAndAoE(new Vector2(3, 4), new Vector2(0, 1));
                    linearRange = false;
                    linearAOE = false;
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

    public Vector2Int[] GetRangeTiles(Vector2Int position) 
    {
        List<Vector2Int> affectedTiles = new List<Vector2Int>();

        //linear
        if (linearRange)
        {
            //go from min range to max range
            for (int i = (int)GetRange().x; i <= (int)GetRange().y; i++)
            {
                //left
                affectedTiles.Add(new Vector2Int(position.x - i, position.y));

                //right
                affectedTiles.Add(new Vector2Int(position.x + i, position.y));
                
                //up
                affectedTiles.Add(new Vector2Int(position.x, position.y + i));
                
                //down
                affectedTiles.Add(new Vector2Int(position.x, position.y - i));
            }
        }
        //non linear
        else
        {
            int weaponMinRange = (int)GetRange().x;
            int weaponMaxRange = (int)GetRange().y;
            //x coordinate
            for (int x = -(int)GetRange().y; x <= (int)GetRange().y; x++)
            {
                //y coordinate
                for (int y = -(int)GetRange().y; y <= (int)GetRange().y; y++)
                {
                    Vector2Int currentPoint = position + new Vector2Int(x, y);
                    int distanceToPoint=(int)Node.CalculateDistance(currentPoint,position);
                    if (distanceToPoint - weaponMinRange >= 0 && distanceToPoint - weaponMaxRange <= 0)
                    {
                        affectedTiles.Add(currentPoint);
                    }
                }
            }
        }

        return affectedTiles.ToArray();
    }

    public Vector2Int[] GetAoeTiles(Vector2Int targetPos, Vector2Int entityPos)
    {
        List<Vector2Int> aoeTiles = new List<Vector2Int>();

        if (linearAOE)
        {
            //pick direction
            Vector2 direction = targetPos - entityPos;
            direction = new Vector2(direction.x / (Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2))),
                direction.y / (Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2))));
            Vector2Int directionInt = new Vector2Int((int)direction.x, (int)direction.y);

            for (int x = 0; x <= (int)GetAreaOfEffect().y; x++)
            {
                aoeTiles.Add(targetPos + (directionInt * x));
            }
        }
        else
        {
            int weaponMinAoe = (int)GetAreaOfEffect().x;
            int weaponMaxAoe = (int)GetAreaOfEffect().y;
            //x coordinate
            for (int x = -(int)GetAreaOfEffect().y; x <= (int)GetAreaOfEffect().y; x++)
            {
                //y coordinate
                for (int y = -(int)GetAreaOfEffect().y; y <= (int)GetAreaOfEffect().y; y++)
                {
                    Vector2Int currentPoint = targetPos + new Vector2Int(x, y);
                    int distanceToPoint = (int)Node.CalculateDistance(currentPoint, targetPos);
                    if (distanceToPoint - weaponMinAoe >= 0 && distanceToPoint - weaponMaxAoe <= 0)
                    {
                        aoeTiles.Add(currentPoint);
                    }
                }
            }
        }
        return aoeTiles.ToArray();
    }

    public void use()
    {

    }
}