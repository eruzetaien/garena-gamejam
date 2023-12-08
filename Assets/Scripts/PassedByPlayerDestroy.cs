using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassedByPlayerDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    public static Transform dumpling;
    public static Vector2 dumplingOffset;
    void Start()
    {
        if (dumpling == null)
        {
            dumpling = GameObject.Find("Dumpling").transform;
            dumplingOffset = new Vector2(-10f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dumpling && transform.position.x < dumpling.position.x + dumplingOffset.x)
        {
            Destroy(gameObject);
        }
    }
}
