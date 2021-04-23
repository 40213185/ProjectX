using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills
{
    public enum SkillList
    {
        None,
        AttackOfOpportunity,
        Execute,
        FireBall,
        IceBall,
        Bleed,
        EagleEye,
        KnockBack
    }
    public SkillList skillType { get; private set; }

    public void SetSkill(SkillList skillChoice)
    {
        skillType = skillChoice;
    }

    public void UseSkill(Weapon weapon, EnemyController enemycontroller, PlayerControllerCombat playercontroller, int damageroll,GameObject user)
    {
        bool applyEnemy = enemycontroller == null ? false : true;
        bool applyPlayer = playercontroller == null ? false : true;
        switch (weapon.skill.skillType)
        {
            case SkillList.AttackOfOpportunity:
                {
                    //roll between 1 and 10
                    int rnd = Random.Range(1, 11);
                    //if any of the first five numbers
                    if (rnd <= 5)
                    {
                        //create a new weapon instance
                        Weapon w = weapon;
                        //in order to modify without modifying main weapon
                        w.ModifyCritChanceBy(0.15f);

                        //crit modifier if any
                        float critMod = 0.0f;
                        //apply to enemy
                        if (applyEnemy)
                        {
                            SoundbankHandler.WeaponAttackType(enemycontroller.gameObject, SoundbankHandler.AttackType.Ability_Attack);
                            SoundbankHandler.WeaponSelector(enemycontroller.gameObject, Weapon.EquipmentType.ArmingSword);
                            SoundbankHandler.SoundEvent(SoundbankHandler.Sounds.Play_Weapons, enemycontroller.gameObject);
                            if (enemycontroller.GetComponent<StatusEffect>())
                                if (enemycontroller.GetComponent<StatusEffect>().GetEffectType() == StatusEffect.EffectType.ExposedToCrit)
                                    critMod = enemycontroller.GetComponent<StatusEffect>().effectPotency / 100;
                        }
                        //apply to player
                        if (applyPlayer)
                        {
                            SoundbankHandler.WeaponAttackType(playercontroller.gameObject, SoundbankHandler.AttackType.Ability_Attack);
                            SoundbankHandler.WeaponSelector(playercontroller.gameObject, Weapon.EquipmentType.ArmingSword);
                            SoundbankHandler.SoundEvent(SoundbankHandler.Sounds.Play_Weapons, playercontroller.gameObject);
                            if (playercontroller.GetComponent<StatusEffect>())
                                if (playercontroller.GetComponent<StatusEffect>().GetEffectType() == StatusEffect.EffectType.ExposedToCrit)
                                    critMod = playercontroller.GetComponent<StatusEffect>().effectPotency / 100;
                        }

                        //and roll for damage
                        int roll = w.RollForDamage(critMod);
                        //apply to enemy
                        if (applyEnemy)
                            enemycontroller.stats.ModifyHealthBy(-roll);
                        if (applyPlayer)
                            playercontroller.stats.ModifyHealthBy(-roll);

                        //log
                        GlobalGameState.UpdateLog(string.Format("<color=red>{0}</color> Attack of Opportunity damage dealt.", roll));
                    }
                    else
                    {
                        //log
                        GlobalGameState.UpdateLog(string.Format("Attack of Opportunity attempt unsuccessful."));
                        if (applyEnemy)
                        {
                            SoundbankHandler.WeaponAttackType(enemycontroller.gameObject, SoundbankHandler.AttackType.Basic_Attack);
                            SoundbankHandler.WeaponSelector(enemycontroller.gameObject, Weapon.EquipmentType.ArmingSword);
                            SoundbankHandler.SoundEvent(SoundbankHandler.Sounds.Play_Weapons, enemycontroller.gameObject);
                        }
                        //apply to player
                        if (applyPlayer)
                        {
                            SoundbankHandler.WeaponAttackType(playercontroller.gameObject, SoundbankHandler.AttackType.Basic_Attack);
                            SoundbankHandler.WeaponSelector(playercontroller.gameObject, Weapon.EquipmentType.ArmingSword);
                            SoundbankHandler.SoundEvent(SoundbankHandler.Sounds.Play_Weapons, playercontroller.gameObject);
                        }
                    }
                    break;
                }
            case SkillList.Execute:
                {
                    //get enemies health in percentage
                    float hpPercentage = enemycontroller.stats.GetCurrentHealth() / enemycontroller.stats.GetMaxHealth();
                    //check and kill
                    if (hpPercentage <= 0.2f)
                    {
                        enemycontroller.Die();
                        //log
                        GlobalGameState.UpdateLog(string.Format("<color=red>{0}</color> succeeded.", "Execute"));
                    }
                    break;
                }
            case SkillList.FireBall:
                {
                    //get affected positions
                    Vector2Int[] affectedPos = new Vector2Int[4];
                    affectedPos[0] = new Vector2Int(Mathf.FloorToInt(enemycontroller.gameObject.transform.position.x),
                        Mathf.FloorToInt(enemycontroller.gameObject.transform.position.z)) + new Vector2Int(0, 1);
                    affectedPos[1] = new Vector2Int(Mathf.FloorToInt(enemycontroller.gameObject.transform.position.x),
                        Mathf.FloorToInt(enemycontroller.gameObject.transform.position.z)) + new Vector2Int(1, 0);
                    affectedPos[2] = new Vector2Int(Mathf.FloorToInt(enemycontroller.gameObject.transform.position.x),
                        Mathf.FloorToInt(enemycontroller.gameObject.transform.position.z)) + new Vector2Int(0, -1);
                    affectedPos[3] = new Vector2Int(Mathf.FloorToInt(enemycontroller.gameObject.transform.position.x),
                        Mathf.FloorToInt(enemycontroller.gameObject.transform.position.z)) + new Vector2Int(-1, 0);
                    //go through combatants
                    foreach (GameObject go in CombatHandler._combatants)
                    {
                        if (go == null) continue;
                        //get the vector2int of the current combatant
                        Vector2Int combatantPos = new Vector2Int(Mathf.FloorToInt(go.transform.position.x),
                            Mathf.FloorToInt(go.transform.position.z));

                        //check against the positions and if it has an enemy controller
                        for (int i = 0; i < affectedPos.Length; i++)
                        {
                            //if found
                            if (affectedPos[i] == combatantPos && go.GetComponent<EnemyController>())
                            {
                                //crit modifier if any
                                float critMod = 0.0f;
                                if (go.GetComponent<StatusEffect>())
                                    if (go.GetComponent<StatusEffect>().GetEffectType() == StatusEffect.EffectType.ExposedToCrit)
                                        critMod = go.GetComponent<StatusEffect>().effectPotency / 100;

                                //take damage
                                int roll = weapon.RollForDamage(critMod);
                                go.GetComponent<EnemyController>().ModifyHealthBy(-roll);

                                //log
                                GlobalGameState.UpdateLog(string.Format("<color=red>{0}</color> fireball damage dealt.", roll));
                            }
                        }
                    }
                    break;
                }
            case SkillList.Bleed:
                {
                    OverwriteOrAddStatusEffect(StatusEffect.EffectType.Bleed, applyPlayer, applyEnemy,
                        playercontroller, enemycontroller, damageroll, user);
                    //log
                    GlobalGameState.UpdateLog(string.Format("It caused <color=red>{0}</color>.", "Bleeding"));
                    break;
                }
            case SkillList.EagleEye:
                {
                    OverwriteOrAddStatusEffect(StatusEffect.EffectType.ExposedToCrit, applyPlayer, applyEnemy,
                        playercontroller, enemycontroller, damageroll, user);     
                    //log
                    GlobalGameState.UpdateLog(string.Format("<color=yellow>{0}</color> locks in to its target.", "Eagle Eye"));

                    break;
                }
            case SkillList.KnockBack:
                {
                    OverwriteOrAddStatusEffect(StatusEffect.EffectType.KnockBack, applyPlayer, applyEnemy,
                        playercontroller, enemycontroller, damageroll, user);     
                    //log
                    GlobalGameState.UpdateLog(string.Format("Attack inflicts <color=yellow>{0}</color>.", "Knock Back"));

                    break;
                }
        }
    }

    private void OverwriteOrAddStatusEffect(StatusEffect.EffectType effectToRemove, bool applyPlayer, bool applyEnemy,
        PlayerControllerCombat playercontroller, EnemyController enemycontroller, int damageroll,GameObject user)
    {
        //overwrites existing status effect
        //or adds new one if one doesnt exist
        bool apply = true;
        //to player
        if (applyPlayer)
        {
            //check overwrite
            if (playercontroller.gameObject.GetComponent<StatusEffect>())
            {
                foreach (StatusEffect se in playercontroller.gameObject.GetComponents<StatusEffect>())
                    if (se.GetEffectType() == effectToRemove)
                    {
                        apply = false;
                        se.setStatusEffect(effectToRemove, StatusEffect.LibraryDuration(effectToRemove),
                StatusEffect.LibraryPotency(effectToRemove, damageroll),user);
                        break;
                    }
            }
            //add new
            if (apply)
            {
                playercontroller.gameObject.AddComponent<StatusEffect>().setStatusEffect(effectToRemove,
                    StatusEffect.LibraryDuration(effectToRemove), StatusEffect.LibraryPotency(effectToRemove, damageroll), user);
            }
        }
        apply = true;
        //to enemy
        if (applyEnemy)
        {
            //check override
            if (enemycontroller.gameObject.GetComponent<StatusEffect>())
            {
                foreach (StatusEffect se in enemycontroller.gameObject.GetComponents<StatusEffect>())
                    if (se.GetEffectType() == effectToRemove)
                    {
                        apply = false;
                        se.setStatusEffect(effectToRemove, StatusEffect.LibraryDuration(effectToRemove),
                StatusEffect.LibraryPotency(effectToRemove, damageroll), user);
                        break;
                    }
            }
            //add new
            if (apply)
            {
                enemycontroller.gameObject.AddComponent<StatusEffect>().setStatusEffect(effectToRemove,
                    StatusEffect.LibraryDuration(effectToRemove), StatusEffect.LibraryPotency(effectToRemove, damageroll), user);
            }
        }
    }
}
