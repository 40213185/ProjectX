using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
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

    public void SetPotency(int min, int max, int floor, int rarity)
    {
        potency.x = min * floor * rarity;
        potency.y = max * floor * rarity;
    }

    public void SetRangeAndAoF(Vector2 newRange, Vector2 newAreaOfEffect)
    {
        range = newRange;
        areaOfEffect = newAreaOfEffect;
    }

    
    public Vector2 GetRange() { return range; }

    public Vector2 GetAreaOfEffect() { return areaOfEffect; }

    public Vector2Int GetPotency() { return potency; }

    public string GetName() { return name; }

    public string GetDescription() { return description; }
}
