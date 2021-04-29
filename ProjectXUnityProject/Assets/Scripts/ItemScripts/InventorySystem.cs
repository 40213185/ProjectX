using System.Collections;
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
        usablesHeld = new Usable[usableSlots];
        if (GameData.CurrentFloor == 0|| equipmentHeld==null)
        {
            equipmentHeld = new Weapon();
            equipmentHeld = Weapon.GetRandomWeapon(100, 100);

            Debug.Log(equipmentHeld.GetName());
        }
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
