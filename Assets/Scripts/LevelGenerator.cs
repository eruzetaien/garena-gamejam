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
        Instantiate(checkpoint, pos + Vector3Int.up * 2, Quaternion.identity);


        // instantiate decorations
        for (int j = 0; j < building.width - 2;)
        {
            // spawn decor
            Building decoration = decorations[Random.Range(0, decorations.Length)];
            if (Random.Range(0, decorRarity) == 0)
            {
                Transform u = Instantiate(decoration.prefab, pos + Vector3Int.right * j + Vector3.up / 2, Quaternion.identity).transform;
                u.SetParent(room);
            }

            j += building.width;
        }

        // instantiate chicken
        for (int j = 0; j < building.width; j++)
        {

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

        levelGenerateY += Random.Range(-buildingYDistance, buildingYDistance + 1);
        levelGenerateY = Mathf.Max(levelGenerateY, 0);
    }
}
