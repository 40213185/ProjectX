using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private string itemName;
    private string description;
    private Vector2 range;
    private Vector2 areaOfEffect;
    private Vector2Int potency;


    public void SetNameAndDesc(string newName, string newDescription)
    {
        itemName = newName;
        description = newDescription;
    }

    public void SetPotency(int min, int max, int multiplier1, int multiplier2)
    {
        potency.x = min * multiplier1 * multiplier2;
        potency.y = max * multiplier1 * multiplier2;
    }

    public void SetRangeAndAoE(Vector2 newRange, Vector2 newAreaOfEffect)
    {
        range = newRange;
        areaOfEffect = newAreaOfEffect;
    }


    public Vector2 GetRange() { return range; }

    public Vector2 GetAreaOfEffect() { return areaOfEffect; }

    public Vector2Int GetPotency() { return potency; }
    public int RollForPotency() 
    {
        return Random.Range(potency.x, potency.y + 1);
    }

    public string GetName() { return itemName; }

    public string GetDescription() { return description; }
}
