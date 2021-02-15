using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventorySystem
{
    public static Weapon[] equipmentHeld { get; private set; }

    public static Usable[] usablesHeld { get; private set; }

    public static int usableSlots;

    public static int equipmentSlots;

}
