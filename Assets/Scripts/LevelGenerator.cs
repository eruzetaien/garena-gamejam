using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [System.Serializable]
    public class Building
    {
        public GameObject prefab;
        public int width;
    }


    // building & decor
    public Building[] buildings;
    public Building[] decorations;

    public int decorRarity = 2;

    public int buildingXDistance = 2;
    public int buildingYDistance = 3;


    int levelGenerateX;
    int levelGenerateY;


    // chicken
    public GameObject chicken;
    public int chickenConsequtive = 5;
    public int chickenRarity = 4;

    int sisaChicken;

    // human
    public GameObject child;
    public GameObject adult;
    public GameObject chef;

    public int chefInterval = 100;
    public int humanRarity = 5;
    public int adultRarity = 4;

    private int progress;
    private int prevHuman;

    // player
    public Transform player;

    // checkpoin
    public GameObject checkpoint;

    void Start()
    {
        // generate random level
        levelGenerateX = 0;
        levelGenerateY = 0;

        sisaChicken = 0;

        // mencegah spawn human di 20 block pertama
        progress = -20;
        
        // play music
        SoundManager.soundManager.PlayLooping("bgm");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.x + 20 > levelGenerateX)
        {
            SpawnBuilding();
        }

    }

    void SpawnBuilding()
    {
        // pick random building
        Building building = buildings[Random.Range(0, buildings.Length)];

        // spawn room collider
        Vector3Int pos = new Vector3Int(levelGenerateX, levelGenerateY, 0);
        Transform room = Instantiate(building.prefab, pos + Vector3.right * (building.width - 1) / 2, Quaternion.identity).transform;


        // spawn checkpoin
        GameObject c = Instantiate(checkpoint, pos + Vector3Int.up * 2, Quaternion.identity);
        c.transform.SetParent(room);


        // instantiate decorations
        for (int j = 1; j < building.width - 2;)
        {
            // spawn decor
            Building decoration = decorations[Random.Range(0, decorations.Length)];
            if (Random.Range(0, decorRarity) == 0)
            {
                Transform u = Instantiate(decoration.prefab, pos + Vector3Int.right * j + Vector3.up / 2, Quaternion.identity).transform;
                u.SetParent(room);
            }

            j += decoration.width;
        }

        // instantiate human
        for (int j = 5; j < building.width - 3; j++)
        {
            progress++;

            if ((progress <= 0 || progress < prevHuman + 10) && progress % chefInterval != 30)
            {
                // nothing happen

            } else {
                // spawn human
                if (progress % chefInterval == 30)
                {
                    // spawn chef
                    Transform u = Instantiate(chef, pos + Vector3Int.right * j + Vector3Int.up *2, Quaternion.identity).transform;
                    u.SetParent(room);

                }
                else if (Random.Range(0, adultRarity) == 0)
                {
                    // spawn adult
                    Transform u = Instantiate(adult, pos + Vector3Int.right * j + Vector3.up *1.2f, Quaternion.identity).transform;
                    u.SetParent(room);

                }
                else
                {
                    // spawn child
                    Transform u = Instantiate(child, pos + Vector3Int.right * j + Vector3Int.up, Quaternion.identity).transform;
                    u.SetParent(room);

                }

                prevHuman = progress;

            }

            // spawn chicken
            if (sisaChicken > 0)
            {
                Transform u = Instantiate(chicken, pos + Vector3Int.right * j + Vector3Int.up, Quaternion.identity).transform;
                u.SetParent(room);
                sisaChicken--;

            }
            else if (Random.Range(0, chickenRarity) == 0)
            {
                sisaChicken = chickenConsequtive;

            }

            
        }

        levelGenerateX += building.width;


        levelGenerateX += buildingXDistance;
        

        if (levelGenerateY == 0)
        {
            levelGenerateX += buildingXDistance * Random.Range(1, 3);
        }

        int ran = Random.Range(-buildingYDistance, buildingYDistance + 1);
        if (ran == 0)
        {
            // no change on level y
            levelGenerateX++;
        }
        levelGenerateY += ran;
        levelGenerateY = Mathf.Max(levelGenerateY, 0);
    }
}
