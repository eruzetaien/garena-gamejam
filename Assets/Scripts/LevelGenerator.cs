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


    public Building[] buildings;
    public Building[] decorations;

    public int decorRarity = 2;

    public int buildingXDistance = 2;
    public int buildingYDistance = 3;


    int levelGenerateX;
    int levelGenerateY;

    void Start()
    {
        // generate random level
        levelGenerateX = 0;
        levelGenerateY = 0;

        for (int i = 0; i < 50; i++)
        {
            // pick random building
            Building building = buildings[Random.Range(0, buildings.Length)];

            // spawn collider
            Vector3Int pos = new Vector3Int(levelGenerateX, levelGenerateY, 0);
            Instantiate(building.prefab, pos + Vector3.right * (building.width-1)/2, Quaternion.identity);


            // instantiate decorations
            for (int j = 0; j < building.width-2;)
            {
                Building decoration = decorations[Random.Range(0, decorations.Length)];
                if (Random.Range(0, decorRarity) == 0)
                {
                    Instantiate(decoration.prefab, pos + Vector3Int.right * j + Vector3Int.up, Quaternion.identity);
                }

                j += decoration.width;
            }
            levelGenerateX += building.width;


            levelGenerateX += buildingXDistance;

            if (levelGenerateY == 0)
            {
                levelGenerateX += buildingXDistance * Random.Range(1, 3);
            }

            levelGenerateY += Random.Range(-buildingYDistance, buildingYDistance+1);
            levelGenerateY = Mathf.Max(levelGenerateY, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
