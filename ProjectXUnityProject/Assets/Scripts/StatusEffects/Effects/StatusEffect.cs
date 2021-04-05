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
    private Vector2Int knockBackPoint;

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

    public void setStatusEffect(EffectType type,int duration,int potency) 
    {
        effectType = type;
        effectDuration = duration;
        currentDuration = effectDuration;
        effectPotency = potency;
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
                    break;
                }
            case EffectType.Freeze:
                {
                    break;
                }
            case EffectType.Bleed:
                {
                    DOT(-effectPotency);
                    break;
                }
            case EffectType.Poisoned:
                {
                    DOT(-effectPotency);
                    break;
                }
            case EffectType.Healing:
                {
                    DOT(effectPotency);
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
        if (currentDuration <= 0) Destroy(this);
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
            //set vector3 of target position
            Vector3 targetPoint = new Vector3(knockBackPoint.x, 0, knockBackPoint.y) + transform.position;
            //move towards it
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, 0.5f * Time.deltaTime);
            //after reached
            if (transform.position == targetPoint)
            {
                //set position point in case of weird float/double points
                transform.position = targetPoint;
                //destroy
                Destroy(this);
            }
        }
    }
}
