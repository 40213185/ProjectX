using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventorySystem
{
    public static Weapon[] equipmentHeld { get; private set; }

    public static Usable[] usablesHeld { get; private set; }

    public static int usableSlots;

    public static int equipmentSlots;

    public static void init(int equipSlots,int useSlots) 
    {
        equipmentSlots = equipSlots;
        usableSlots = useSlots;
        equipmentHeld = new Weapon[equipmentSlots];
        usablesHeld = new Usable[usableSlots];
        Debug.Log("Inventory Init");
        usablesHeld[0] = new Usable(Usable.UsableType.Potion);
        usablesHeld[1] = new Usable(Usable.UsableType.Bomb);
        Debug.Log(usablesHeld[1].GetName());
    }

    public static void AddToInventory(Weapon equip,int index) 
    {
        equipmentHeld[index] = equip;
    }

    public static void AddToInventory(Usable usable,int index)
    {
        usablesHeld[index] = usable;
    }
}
