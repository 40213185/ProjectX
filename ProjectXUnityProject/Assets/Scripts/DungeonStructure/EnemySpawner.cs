using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HideIfPlayerNotPresent))]

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyList;
    public int enemySpawnMin, enemySpawnMax;

    public void CheckAndPlaceEnemies()
    {
        //only if enemylist is not empty
        if (enemyList != null)
        {
            //create possible spawning points list
            List<Vector2> possibleSpawningPoints = new List<Vector2>();

            //get room player is currently at
            Vector2Int currentRoom = new Vector2Int(
                Mathf.FloorToInt((GameObject.FindGameObjectWithTag("Player").transform.position.x + 1) / MapHandler.roomSizex),
                Mathf.FloorToInt((GameObject.FindGameObjectWithTag("Player").transform.position.z + 1) / MapHandler.roomSizey));

            //check walkables
            for (int y = currentRoom.y * MapHandler.roomSizey; y < (currentRoom.y * MapHandler.roomSizey) + MapHandler.roomSizey; y++)
            {
                for (int x = currentRoom.x * MapHandler.roomSizex; x < (currentRoom.x * MapHandler.roomSizex) + MapHandler.roomSizex; x++)
                {
                    //if walkable
                    if (MapHandler.GetTileTypeFromMatrix(new Vector2Int(x, y)) == MapHandler.TileType.Walkable)
                        //add to list
                        possibleSpawningPoints.Add(new Vector2(x, y));
                }
            }

            //roll between min and max
            int rnd = Random.Range(enemySpawnMin, enemySpawnMax + 1);
            //create array accoridingly+player
            GameObject[] combatants = new GameObject[rnd+1];

            Debug.Log("Spawning enemies");
            for (int i = 0; i < rnd; i++)
            {
                //get random index
                int index = Random.Range(0, possibleSpawningPoints.Count);

                //choose spawn point with index
                Vector2 spawnPoint = possibleSpawningPoints[index];
                Debug.Log("Choosing enemy");
                //choose enemy
                GameObject spawnable = enemyList[Random.Range(0, enemyList.Length)];
                Debug.Log("Placing enemy");
                //instantiate it
                combatants[i] = Instantiate(spawnable, new Vector3(spawnPoint.x, 0, spawnPoint.y), spawnable.transform.rotation);
                Debug.Log("REmoving from list");
                //remove from list for next spawn
                possibleSpawningPoints.RemoveAt(index);
                Debug.Log("Removed");
            }

            //add player to combatants list
            combatants[combatants.Length - 1] = GameObject.FindGameObjectWithTag("Player");

            //enemies spawned? start a fight
            CombatHandler.StartCombat(combatants);

            //disable component after use
            enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

}
