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
        Bleed,
        EagleEye,
        KnockBack
    }
    public SkillList skillType { get; private set; }

    public void SetSkill(SkillList skillChoice)
    {
        skillType = skillChoice;
    }

    public void UseSkill(Weapon weapon,EnemyController enemycontroller) 
    {
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
                        if (enemycontroller.GetComponent<StatusEffect>())
                            if (enemycontroller.GetComponent<StatusEffect>().GetEffectType() == StatusEffect.EffectType.ExposedToCrit)
                                critMod = enemycontroller.GetComponent<StatusEffect>().effectPotency / 100;

                        //and roll for damage
                        enemycontroller.stats.ModifyHealthBy(-w.RollForDamage(critMod));
                    }
                    break;
                }
            case SkillList.Execute:
                {
                    //get enemies health in percentage
                    float hpPercentage = enemycontroller.stats.GetCurrentHealth() / enemycontroller.stats.GetMaxHealth();
                    //check and kill
                    if (hpPercentage <= 0.2f) enemycontroller.Die();
                    break;
                }
            case SkillList.FireBall:
                {
                    //get affected positions
                    Vector2Int[] affectedPos = new Vector2Int[4];
                    affectedPos[0] = new Vector2Int(Mathf.FloorToInt(enemycontroller.gameObject.transform.position.x),
                        Mathf.FloorToInt(enemycontroller.gameObject.transform.position.z))+new Vector2Int(0,1);
                    affectedPos[1] = new Vector2Int(Mathf.FloorToInt(enemycontroller.gameObject.transform.position.x),
                        Mathf.FloorToInt(enemycontroller.gameObject.transform.position.z)) + new Vector2Int(1, 0);
                    affectedPos[2] = new Vector2Int(Mathf.FloorToInt(enemycontroller.gameObject.transform.position.x),
                        Mathf.FloorToInt(enemycontroller.gameObject.transform.position.z)) + new Vector2Int(0, -1);
                    affectedPos[3] = new Vector2Int(Mathf.FloorToInt(enemycontroller.gameObject.transform.position.x),
                        Mathf.FloorToInt(enemycontroller.gameObject.transform.position.z)) + new Vector2Int(-1, 0);
                    //go through combatants
                    foreach (GameObject go in CombatHandler._combatants)
                    {
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
                                go.GetComponent<EnemyController>().ModifyHealthBy(-weapon.RollForDamage(critMod));
                            }
                        }
                    }
                    break;
                }
            case SkillList.Bleed:
                {

                    break;
                }
            case SkillList.EagleEye:
                {
                    break;
                }
            case SkillList.KnockBack:
                {
                    break;
                }
        }
    }
}
