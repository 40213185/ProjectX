using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public enum EquipmentType {
        ArmingSword,
        Halberd,
        Greatsword,
        SpellBook,
        Flintlock,
        Dagger
    }

    private int rarity;
    private EquipmentType type;
    public Skills skill { get; private set; }
    private StatusEffect.EffectType effect;
    private int cost;
    private bool linearRange;
    private bool linearAOE;
    private float critChance;
    private float critModifier;
    private float missChance;

    public Weapon()
    {
        skill = new Skills();
        critModifier = 2.5f;
        critChance = 0.1f;
        missChance = 0.05f;
    }

    public static Weapon GetRandomWeapon(int modifier1, int modifier2)
    {
        Weapon weaponRoll = new Weapon();
        int rndType = Random.Range(0, 6);
        EquipmentType equipType = EquipmentType.ArmingSword;
        switch (rndType)
        {
            case 0: { equipType = EquipmentType.ArmingSword; break; }
            case 1: { equipType = EquipmentType.Halberd; break; }
            case 2: { equipType = EquipmentType.Greatsword; break; }
            case 3: { equipType = EquipmentType.SpellBook; break; }
            case 4: { equipType = EquipmentType.Flintlock; break; }
            case 5: { equipType = EquipmentType.Dagger; break; }
        }
        switch (equipType)
        {
            case EquipmentType.ArmingSword:
                weaponRoll.type = EquipmentType.ArmingSword;
                weaponRoll.skill.SetSkill(Skills.SkillList.AttackOfOpportunity);
                weaponRoll.effect = StatusEffect.EffectType.None;
                weaponRoll.SetNameAndDesc("Arming Sword", "A long bladed weapon, Makes a nice whoosh sound when used in a slashing motion");
                weaponRoll.SetPotency(1, 3, modifier2, modifier1);
                weaponRoll.rarity = modifier2;
                weaponRoll.SetRangeAndAOEforSkill(weaponRoll.type);
                weaponRoll.cost = 3;
                break;

            case EquipmentType.Halberd:
                weaponRoll.type = EquipmentType.Halberd;
                weaponRoll.skill.SetSkill(Skills.SkillList.Execute);
                weaponRoll.effect = StatusEffect.EffectType.None;
                weaponRoll.SetNameAndDesc("Halberd", "Its like an axe and a spear had a child, Good for poking but also chopping heads");
                weaponRoll.SetPotency(2, 5, modifier2, modifier1);
                weaponRoll.rarity = modifier2;
                weaponRoll.SetRangeAndAOEforSkill(weaponRoll.type);
                weaponRoll.cost = 4;
                break;

            case EquipmentType.Greatsword:
                weaponRoll.type = EquipmentType.Greatsword;
                weaponRoll.skill.SetSkill(Skills.SkillList.KnockBack);
                weaponRoll.effect = StatusEffect.EffectType.None;
                weaponRoll.SetNameAndDesc("Greatsword", "A Massive sword, Surprised you can even hold it up, Good at cracking eggs");
                weaponRoll.SetPotency(4, 8, modifier2, modifier1);
                weaponRoll.rarity = modifier2;
                weaponRoll.SetRangeAndAOEforSkill(weaponRoll.type);
                weaponRoll.cost = 5;
                break;

            case EquipmentType.SpellBook:
                weaponRoll.type = EquipmentType.SpellBook;
                weaponRoll.skill.SetSkill(Skills.SkillList.FireBall);
                weaponRoll.effect = StatusEffect.EffectType.None;
                weaponRoll.SetNameAndDesc("Fireball", "Summon a fireball from god knows where and use it for things such as cooking or other activities");
                weaponRoll.SetPotency(3, 6, modifier2, modifier1);
                weaponRoll.rarity = modifier2;
                weaponRoll.SetRangeAndAOEforSkill(weaponRoll.type);
                weaponRoll.cost = 4;
                break;

            case EquipmentType.Flintlock:
                weaponRoll.type = EquipmentType.Flintlock;
                weaponRoll.skill.SetSkill(Skills.SkillList.EagleEye);
                weaponRoll.effect = StatusEffect.EffectType.None;
                //weaponRoll.SetNameAndDesc("Icespike", "Summon a giant ice spike that can be launched like a rocket, remember to wear gloves when using this, can get chilly");
                weaponRoll.SetNameAndDesc("Flintlock", "A handheld musket?!? Now you can take on your enemies from afar with little effort");
                weaponRoll.SetPotency(3, 6, modifier2, modifier1);
                weaponRoll.rarity = modifier2;
                weaponRoll.SetRangeAndAOEforSkill(weaponRoll.type);
                weaponRoll.cost = 3;
                weaponRoll.ModifyCritChanceBy(0.1f);
                break;

            case EquipmentType.Dagger:
                weaponRoll.type = EquipmentType.Dagger;
                weaponRoll.skill.SetSkill(Skills.SkillList.Bleed);
                weaponRoll.effect = StatusEffect.EffectType.Bleed;
                weaponRoll.SetNameAndDesc("Dagger", "A Small bladed dagger, used for stabbing things, Good at making holes that bleed");
                weaponRoll.SetPotency(3, 6, modifier2, modifier1);
                weaponRoll.rarity = modifier2;
                weaponRoll.SetRangeAndAOEforSkill(weaponRoll.type);
                weaponRoll.cost = 2;
                weaponRoll.ModifyCritChanceBy(0.4f);
                break;
        }

        return weaponRoll;
    }
    public void CreateWeapon(EquipmentType equipType, Skills.SkillList equipSkill, int usecost, int minPotency, int maxPotency, int modifier1, int modifier2)
    {
        type = equipType;
        skill.SetSkill(equipSkill);
        effect = StatusEffect.EffectType.None;
        SetRangeAndAOEforSkill(type);
        SetPotency(minPotency, maxPotency, modifier2, modifier1);
        cost = usecost;
    }
    public void CreateWeapon(EquipmentType equipType, Skills.SkillList equipSkill, StatusEffect.EffectType effectType, int usecost, int minPotency, int maxPotency, int modifier1, int modifier2)
    {
        type = equipType;
        skill.SetSkill(equipSkill);
        effect = effectType;
        SetRangeAndAOEforSkill(type);
        SetPotency(minPotency, maxPotency, modifier2, modifier1);
        cost = usecost;
    }

    private void SetRangeAndAOEforSkill(EquipmentType equipType)
    {
        switch (equipType)
        {
            case EquipmentType.ArmingSword:
                {
                    SetRangeAndAoE(new Vector2(1, 2), new Vector2(0, 0));
                    linearRange = true;
                    linearAOE = true;
                    break;
                }
            case EquipmentType.Dagger:
                {
                    SetRangeAndAoE(new Vector2(1, 1), new Vector2(0, 0));
                    linearRange = true;
                    linearAOE = true;
                    break;
                }
            case EquipmentType.Flintlock:
                {
                    SetRangeAndAoE(new Vector2(1, 5), new Vector2(0, 0));
                    linearRange = false;
                    linearAOE = false;
                    break;
                }
            case EquipmentType.Greatsword:
                {
                    SetRangeAndAoE(new Vector2(1, 1), new Vector2(0, 1));
                    linearRange = true;
                    linearAOE = true;
                    break;
                }
            case EquipmentType.Halberd:
                {
                    SetRangeAndAoE(new Vector2(1, 3), new Vector2(0, 0));
                    linearRange = true;
                    linearAOE = true;
                    break;
                }
            case EquipmentType.SpellBook:
                {
                    SetRangeAndAoE(new Vector2(1, 4), new Vector2(0, 0));
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

    public StatusEffect.EffectType GetEffectType() 
    {
        return effect;
    }

    public void ModifyCritChanceBy(float amount)
    {
        critChance += amount;
    }

    public int RollForDamage()
    {
        Vector2Int damageVector = GetPotency();

        int damage = Random.Range(damageVector.x, damageVector.y);

        //miss chance
        if (Random.Range(0.0f, 1.0f) < missChance)
        {
            damage = 0;
            GlobalGameState.UpdateLog("Attack missed.");
        }
        //apply crit rate
        else if (Random.Range(0.0f, 1.0f) < critChance)
        {
            damage = Mathf.CeilToInt(damage * critModifier);
            GlobalGameState.UpdateLog("Critical hit.");
        }

        return damage;
    }
    public int RollForDamage(float critIncrment)
    {
        Vector2Int damageVector = GetPotency();

        int damage = Random.Range(damageVector.x, damageVector.y);

        //miss chance
        if (Random.Range(0.0f, 1.0f) < missChance)
        {
            damage = 0;
            GlobalGameState.UpdateLog("Attack <color=gray>missed</color>.");
        }
        //apply crit rate
        else if (Random.Range(0.0f, 1.0f) < critChance + critIncrment)
        {
            damage = Mathf.CeilToInt(damage * critModifier);
            if(damage>0)
                GlobalGameState.UpdateLog("<color=red>Critical</color> hit.");
        }

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
}