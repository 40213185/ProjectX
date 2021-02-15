﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    public enum EffectType 
    {
        None,
        Burn,
        Freeze,
        Poisoned,
        Bleed,
        Stun,
        Healing,
        StrengthBuff,
        IntBuff
    }
    private EffectType effectType;
    public int effectDuration;
    private int currentDuration;
    private int currentTurn;

    public StatusEffect(EffectType type,int duration) 
    {
        effectType = type;
        effectDuration = duration;
        currentDuration = effectDuration;
        currentTurn = GlobalGameState.turnTime;
    }

    private void TurnTick() 
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

    public void FixedUpdate()
    {
        if (GlobalGameState.turnTime != currentTurn) TurnTick();
    }
}
