using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    public enum EffectType 
    {
        Burn,
        Freeze,
        Poisoned,
        Bleed,
        Healing,
        StrengthBuff,
        IntBuff,
        Stun,
        None
    }
    private EffectType effectType;
    public int effectDuration;
    private int currentDuration;

    public StatusEffect(EffectType type,int duration) 
    {
        effectType = type;
        effectDuration = duration;
        currentDuration = effectDuration;
    }

    public void TurnTick() 
    {
        //apply effect on turn tick
        ///
        ///code here. Switch case?
        ///
        currentDuration--;
    }

    public EffectType GetEffectType() 
    {
        return effectType;
    }
    public int GetCurrentDuration() 
    {
        return currentDuration;
    }

}
