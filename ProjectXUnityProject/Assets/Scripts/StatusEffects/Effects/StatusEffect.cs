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
        None
    }
    private EffectType effectType;
    public int effectDuration { get; private set; }
    private int currentDuration;
    public int effectPotency { get; private set; }


    public void setStatusEffect(EffectType type,int duration,int potency) 
    {
        effectType = type;
        effectDuration = duration;
        currentDuration = effectDuration;
        effectPotency = potency;
    }

    public void TurnTick() 
    {
        //apply effect on turn tick
        ///
        ///code here. Switch case?
        ///
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

}
