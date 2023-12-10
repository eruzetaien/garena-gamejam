using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cobblestone : MonoBehaviour
{
    [Header("Cobblestone")]
    public int y;
    public Tile cobblestoneTile;
    public Tile undergroundTile;

    Tilemap tilemap;
    Transform player;

    [Header("Paralax Background")]
    public GameObject paralax;
    public float yBackground;
    public Sprite[] bgSprites;

    List<GameObject> instantiatedBg;

    private float nextX;


    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        player = GameObject.FindWithTag("Player").transform;
        instantiatedBg = new List<GameObject>();

        nextX = -20;
    }


    void Update()
    {
        // Cobblestone
        tilemap.ClearAllTiles();
        for (int x = Mathf.FloorToInt(player.position.x - 30); x <= Mathf.CeilToInt(player.position.x + 30); x++)
        {
            tilemap.SetTile(new Vector3Int(x, y, 0), cobblestoneTile);

            for (int i = y-5; i<y; i++)
            {
                tilemap.SetTile(new Vector3Int(x, i, 0), undergroundTile);
            }
            
        }

        // Paralax background
        if (player.position.x > nextX - 30)
        {
            GameObject a = Instantiate(paralax, new Vector3(nextX, yBackground, 5), Quaternion.identity);
            a.GetComponent<SpriteRenderer>().sprite = bgSprites[Random.Range(0, bgSprites.Length)];

            instantiatedBg.Add(a);

            nextX += Random.Range(3, 10);
        }

        // remove uneeded bg
        if (instantiatedBg.Count > 0 && instantiatedBg[0].transform.position.x < player.position.x - 30)
        {
            Destroy(instantiatedBg[0]);
            instantiatedBg.RemoveAt(0);
        }
    }
}
