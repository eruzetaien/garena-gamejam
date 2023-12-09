using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cobblestone : MonoBehaviour
{
    public int y;
    public Tile cobblestoneTile;
    public Tile undergroundTile;

    Tilemap tilemap;
    Transform player;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        player = GameObject.FindWithTag("Player").transform;
    }


    void Update()
    {
        tilemap.ClearAllTiles();
        for (int x = Mathf.FloorToInt(player.position.x - 30); x <= Mathf.CeilToInt(player.position.x + 30); x++)
        {
            tilemap.SetTile(new Vector3Int(x, y, 0), cobblestoneTile);

            for (int i = y-5; i<y; i++)
            {
                tilemap.SetTile(new Vector3Int(x, i, 0), undergroundTile);
            }
            
        }
    }
}
