﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventorySystem
{
    public static Weapon equipmentHeld { get; private set; }

    public static Usable[] usablesHeld { get; private set; }

    public static int usableSlots;

    public static void init(int useSlots) 
    {
        usableSlots = useSlots;
        equipmentHeld = new Weapon();
        usablesHeld = new Usable[usableSlots];
        equipmentHeld = Weapon.GetRandomWeapon(100,100);
        Debug.Log(string.Format("{0}\n{1}\n{2}",
            equipmentHeld.GetEquipmentType().ToString(),equipmentHeld.skill.skillType.ToString(),equipmentHeld.GetEffectType().ToString()));
    }

    public static void ReplaceWeapon(Weapon equip)
    {
        equipmentHeld = equip;
    }
    public static void AddToInventory(Usable usable,int index)
    {
        usablesHeld[index] = usable;
    }
}
