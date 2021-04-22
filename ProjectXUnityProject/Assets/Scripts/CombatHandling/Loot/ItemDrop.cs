using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Weapon droppedWep;
    private bool isWep;
    private Usable droppedUsable;
    public GameObject dropObject;


    public void GenerateLoot()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        isWep = false;
        int randomNum = Random.Range(0,100);
        Debug.Log("RandomNum = "+randomNum);
        if(randomNum > 70) 
        {
            Random.seed = System.DateTime.Now.Millisecond;
            randomNum = Random.Range(0, 100);
            Debug.Log(randomNum);

            if(randomNum > 95) 
            {
                droppedWep = Weapon.GetRandomWeapon(GameData.CurrentFloor, 4);
                Debug.Log("RAREST GOING SHIZ");
            }
            if(randomNum > 80 && randomNum < 95) 
            {
                droppedWep = Weapon.GetRandomWeapon(GameData.CurrentFloor, 3);
            }
            if(randomNum > 60 && randomNum < 80) 
            {
                droppedWep = Weapon.GetRandomWeapon(GameData.CurrentFloor, 2);
            }
            if(randomNum < 60) 
            {
                droppedWep = Weapon.GetRandomWeapon(GameData.CurrentFloor, 1);
            }
            isWep = true;
        }
        else 
        {
            droppedUsable = new Usable(Usable.UsableType.Potion);
        }
        
    }
   

    public void chanceToDrop(Vector3 pos) 
    {
        Random.seed = System.DateTime.Now.Millisecond;
        int chance = Random.Range(0, 100);
        if(chance > 0) 
        {
            GenerateLoot();
            GameObject droppedObject = Instantiate(dropObject, pos, dropObject.transform.rotation);
            if (isWep) 
            {
                droppedObject.GetComponent<dropScript>().getWeapon(droppedWep);
                Debug.Log(droppedWep.GetName() + droppedWep.GetPotency() + droppedWep.getRarity());
                GlobalGameState.UpdateLog("Dropped a " + droppedWep.GetName());
            }
            else 
            {
                droppedObject.GetComponent<dropScript>().getUsable(droppedUsable);
                GlobalGameState.UpdateLog("Dropped a " + droppedUsable.GetName());
            }
            
        }
    }

}
