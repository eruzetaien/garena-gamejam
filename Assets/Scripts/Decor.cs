
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decor : MonoBehaviour
{

    Rigidbody2D rb;

    bool kicked;
    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        kicked = false;
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !kicked)
        {
            Debug.Log("hut");
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(new Vector2(Random.Range(1, 9), Random.Range(1, 5)), ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(0, -45));

            gameObject.layer = LayerMask.NameToLayer("Decor");

            kicked = true;
        }
    }
}
