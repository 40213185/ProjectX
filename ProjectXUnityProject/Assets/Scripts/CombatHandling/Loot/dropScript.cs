using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropScript : MonoBehaviour
{
    private Weapon droppedWep;
    private Usable droppedUsable;

    private int rarity;

    public GameObject parent;
    public GameObject[] drops;

    public GameObject particleSys;

    public Weapon.EquipmentType[] weaponTypes;
    public Usable.UsableType[] usableTypes;

    private Weapon sortSpaceWep;
    private bool mouseOver = false;
    private bool isWep;

    public void getWeapon(Weapon item) 
    {
        try
        {
            droppedWep = item;
            int index = 0;
            for (int i = 0; i < 6; i++)
            {
                if (droppedWep.GetEquipmentType() == weaponTypes[i])
                {
                    index = i;
                }
            }
            switch (droppedWep.GetEquipmentType()) 
            {
                case (Weapon.EquipmentType.ArmingSword):
                    Instantiate(drops[0], parent.transform);
                    break;
                case (Weapon.EquipmentType.Halberd):
                    Instantiate(drops[1], parent.transform);
                    break;
                case (Weapon.EquipmentType.Greatsword):
                    Instantiate(drops[2], parent.transform);
                    break;
                case (Weapon.EquipmentType.SpellBook):
                    Instantiate(drops[3], parent.transform);
                    break;
                case (Weapon.EquipmentType.Flintlock):
                    Instantiate(drops[4], parent.transform);
                    break;
                case (Weapon.EquipmentType.Dagger):
                    Instantiate(drops[5], parent.transform);
                    break;
            }
            
            rarity = droppedWep.getRarity();
            setRarityColour();
            isWep = true;
        }catch(Exception e) 
        {
            Debug.LogError(e);
            GameObject.Destroy(this.gameObject);
        }
    }

    public void OnMouseEnter()
    {
        //Enable ability to be clicked
        mouseOver = true;
        //ShowTooltip    
        if (isWep)
        {
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().ShowToolTipWeapon(droppedWep.GetEquipmentType(), droppedWep.GetName(), droppedWep.getRarity(), droppedWep.GetPotency(), droppedWep.GetDescription());
            
        }
        else 
        {
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().ShowToolTipUsable(droppedUsable.GetUsableType(), droppedUsable.GetName(), droppedUsable.GetPotency(), droppedUsable.GetDescription());
        }  
    }

    public void OnMouseExit()
    {
        //Disable ability to be clicked
        mouseOver = false;
        //HideToolTip
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().HideToolTip();
    }

    private void OnMouseDown()
    {
        //If dropped item is a weapon
        if (isWep)
        {
            //If player is in ranged and item is pressed swap items
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position) <= 1 && mouseOver)
            {
                //Hold the players current weapon in temp space
                sortSpaceWep = InventorySystem.equipmentHeld;
                //Replace Players current Weapon with the new Weapon
                InventorySystem.ReplaceWeapon(droppedWep);
                //Replace the dropped Weapon with the players old Weapon
                droppedWep = sortSpaceWep;
                //Make sort space empty incase player wishes to swap weapons again
                sortSpaceWep = null;
                //Update the Tooltip to show the old weapon on the floor
                GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().HideToolTip();
                GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().ShowToolTipWeapon(droppedWep.GetEquipmentType(), droppedWep.GetName(), droppedWep.getRarity(), droppedWep.GetPotency(), droppedWep.GetDescription());
                //Change effects on dropped weapon incase rarity is different between old and new weapon
                rarity = droppedWep.getRarity();
                setRarityColour();
            }
        }
        else 
        {
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position) <= 1 && mouseOver)
            {
                if(InventorySystem.usablesHeld.Length < 5) 
                {
                     for(int i = 0; i < 4; i++) 
                    {
                        if (InventorySystem.usablesHeld[i] == null) 
                        {
                            InventorySystem.AddToInventory(droppedUsable, i);
                            GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().HideToolTip();
                            GameObject.Destroy(this.gameObject);
                            break;
                        }
                    }
                }
                else 
                {
                    GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().toolTipMessage("Inventory Full");
                }

            }
        }
    }

    public void getUsable(Usable item) 
    {
        droppedUsable = item;

        int index = 0;
        for (int i = 0; i < 2; i++)
        {
            if (droppedUsable.GetUsableType() == usableTypes[i])
            {
                index = i;
                break;
            }
        }
        Instantiate(drops[6], parent.transform);
        particleSys.GetComponent<ParticleSystem>().startColor = Color.cyan;
        isWep = false;
        if (droppedUsable == null) GameObject.Destroy(this.gameObject);
    }

    public void setRarityColour()
    {
        switch (rarity) 
        {
            case 1:
                particleSys.GetComponent<ParticleSystem>().startColor = Color.white;
                break;
            case 2:
                particleSys.GetComponent<ParticleSystem>().startColor = Color.green;
                break;
            case 3:
                particleSys.GetComponent<ParticleSystem>().startColor = Color.blue;
                break;
            case 4:
                particleSys.GetComponent<ParticleSystem>().startColor = Color.yellow;
                break;
        }
    }


}
