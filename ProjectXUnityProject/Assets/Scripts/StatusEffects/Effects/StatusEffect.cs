using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    public enum EffectType 
    {
        Burn,
        Freeze,
        Bleed,
        Poisoned,
        Healing,
        StrengthBuff,
        IntBuff,
        Stun,
        KnockBack,
        ExposedToCrit,
        None
    }
    private EffectType effectType;
    public int effectDuration { get; private set; }
    private int currentDuration;
    public int effectPotency { get; private set; }
    private bool setKnockbackPoint;
    private Vector2Int knockBackPoint;
    private GameObject usedBy;

    public void SetUser(GameObject user) 
    {
        usedBy = user;
    }

    public static int LibraryDuration(EffectType type)
    {
        int amount = 0;
        switch (type)
        {
            case EffectType.Burn:
                {
                    amount = 3;
                    break;
                }
            case EffectType.Freeze:
                {
                    amount = 2;
                    break;
                }
            case EffectType.Bleed:
                {
                    amount = 3;
                    break;
                }
            case EffectType.Poisoned:
                {
                    amount = 3;
                    break;
                }
            case EffectType.Healing:
                {
                    amount = 0;
                    break;
                }
            case EffectType.StrengthBuff:
                {
                    amount = 3;
                    break;
                }
            case EffectType.IntBuff:
                {
                    amount = 3;
                    break;
                }
            case EffectType.Stun:
                {
                    amount = 1;
                    break;
                }
            case EffectType.KnockBack:
                {
                    amount = 0;
                    break;
                }
            case EffectType.ExposedToCrit:
                {
                    amount = 2;
                    break;
                }
            default:
                {
                    amount = 0;
                    break;
                }
        }
        return amount;
    }
    public static int LibraryPotency(EffectType type,int modifier)
    {
        int amount = 0;
        switch (type)
        {
            case EffectType.Burn:
                {
                    amount = Mathf.CeilToInt(modifier/4);
                    break;
                }
            case EffectType.Freeze:
                {
                    amount = Mathf.CeilToInt(modifier / 10);
                    break;
                }
            case EffectType.Bleed:
                {
                    amount = Mathf.CeilToInt(modifier / 3);
                    break;
                }
            case EffectType.Poisoned:
                {
                    amount = Mathf.CeilToInt(modifier / 5);
                    break;
                }
            case EffectType.Healing:
                {
                    amount = Mathf.CeilToInt(modifier);
                    break;
                }
            case EffectType.StrengthBuff:
                {
                    amount = Mathf.CeilToInt(modifier);
                    break;
                }
            case EffectType.IntBuff:
                {
                    amount = Mathf.CeilToInt(modifier);
                    break;
                }
            case EffectType.Stun:
                {
                    amount = 0;
                    break;
                }
            case EffectType.KnockBack:
                {
                    amount = 0;
                    break;
                }
            case EffectType.ExposedToCrit:
                {
                    amount = 0;
                    break;
                }
            default:
                {
                    amount = 0;
                    break;
                }
        }
        return amount;
    }

    public void setStatusEffect(EffectType type,int duration,int potency,GameObject user) 
    {
        effectType = type;
        effectDuration = duration;
        currentDuration = effectDuration;
        effectPotency = potency;

        setKnockbackPoint = false;

        SetUser(user);
    }

    private void DOT(int value) 
    {
        if (gameObject.GetComponent<PlayerControllerCombat>())
            gameObject.GetComponent<PlayerControllerCombat>().stats.ModifyHealthBy(value);
        else if (gameObject.GetComponent<EnemyController>())
            gameObject.GetComponent<EnemyController>().ModifyHealthBy(value);
    }

    public void TurnTick() 
    {
        //apply effect on turn tick
        switch (effectType)
        {
            case EffectType.Burn:
                {
                    DOT(-effectPotency);
                    GlobalGameState.UpdateLog(string.Format("Took <color=red>{0}</color> <color=red>{1}</color> damage.",effectPotency,"Burn"));
                    break;
                }
            case EffectType.Freeze:
                {
                    break;
                }
            case EffectType.Bleed:
                {
                    DOT(-effectPotency);
                    GlobalGameState.UpdateLog(string.Format("Took <color=red>{0}</color> <color=red>{1}</color> damage.", effectPotency, "Bleed"));
                    break;
                }
            case EffectType.Poisoned:
                {
                    DOT(-effectPotency);
                    GlobalGameState.UpdateLog(string.Format("Took <color=red>{0}</color> <color=purpl>{1}</color> damage.", effectPotency, "Poison"));
                    break;
                }
            case EffectType.Healing:
                {
                    DOT(effectPotency);
                    GlobalGameState.UpdateLog(string.Format("Healed <color=green>{0}</color> health.", effectPotency));
                    break;
                }
            case EffectType.StrengthBuff:
                {
                    break;
                }
            case EffectType.IntBuff:
                {
                    break;
                }
            case EffectType.Stun:
                {
                    if (gameObject.GetComponent<PlayerControllerCombat>())
                        gameObject.GetComponent<PlayerControllerCombat>().EndTurn();
                    else if (gameObject.GetComponent<EnemyController>())
                        gameObject.GetComponent<EnemyController>().EndTurn();
                    GlobalGameState.UpdateLog(string.Format("{0} is <color=yellow>{1}</color>. Skipping turn.", name, "Stunned"));
                    break;
                }
            case EffectType.KnockBack:
                {
                    //nothing really happens here
                    break;
                }
            case EffectType.ExposedToCrit:
                {
                    //nothing really happens here
                    break;
                }
        }
        currentDuration--;
        //remove after duration is done
        if (currentDuration <= 0)
        {
            GlobalGameState.UpdateLog(string.Format("<color=yellow>{0}</color> on {1} expired.", effectType, name));
            Destroy(this);
        }
    }

    public EffectType GetEffectType() 
    {
        return effectType;
    }
    public int GetCurrentDuration() 
    {
        return currentDuration;
    }

    public void SetKnockBackPoint(Vector2Int point) 
    {
        knockBackPoint = point;
    }

    public void FixedUpdate()
    {
        if (effectType == EffectType.None) Destroy(this);
        if (effectType == EffectType.KnockBack)
        {
            if (!setKnockbackPoint&&usedBy!=null)
            {
                Vector3 current = transform.position;
                Vector3 target = usedBy.transform.position;
                Vector3 direction = new Vector3(current.x - target.x,0, current.z - target.z);

                //knockback 2 cells
                direction =direction.normalized * 2;

                Vector2Int point = new Vector2Int(Mathf.FloorToInt(direction.x+transform.position.x),
                    Mathf.FloorToInt(direction.z+transform.position.z));

                SetKnockBackPoint(point);

                setKnockbackPoint = true;
            }

            if (setKnockbackPoint) {
                //set vector3 of target position
                Vector3 targetPoint = new Vector3(knockBackPoint.x, 0, knockBackPoint.y);
                //and next position
                //move towards it
                Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetPoint, 5f * Time.deltaTime);

                //check if not walkable
                if (MapHandler.GetTileTypeFromPosition(nextPosition) != MapHandler.TileType.Walkable)
                {
                    //not walkable, set target position to current position
                    targetPoint = transform.position;
                }
                else
                {
                    //get rounded position for comparison with combatants
                    Vector2Int roundedPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
                    //check if enemies in that position
                    foreach (GameObject co in CombatHandler._combatants)
                    {
                        if (co != null && co != gameObject)
                        {
                            //get rounded position for comparison
                            Vector2Int roundedPosCo = new Vector2Int(Mathf.RoundToInt(co.transform.position.x), Mathf.RoundToInt(co.transform.position.z));
                            if (roundedPos == roundedPosCo)
                            {
                                targetPoint = transform.position;
                            }
                        }
                    }

                    if (targetPoint != transform.position)
                    {
                        transform.position = nextPosition;
                    }
                    else 
                    {
                        //after reached
                        if (transform.position == targetPoint)
                        {
                            //set position point in case of weird float/double points
                            transform.position = new Vector3(Mathf.FloorToInt(transform.position.x), transform.position.y,
                                Mathf.FloorToInt(transform.position.z));
                            //destroy
                            Destroy(this);
                        }
                    }
                }
            }
        }
    }
}
