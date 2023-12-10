
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

            rb.bodyType = RigidbodyType2D.Dynamic;

            float power = 1;
            switch (collision.GetComponent<Dumpling>().chickenState)
            {
                case Dumpling.ChickenState.STATE_1:
                    power = 1;
                    break;
                case Dumpling.ChickenState.STATE_2:
                    power = 3f;
                    break;
                case Dumpling.ChickenState.STATE_3:
                    power = 6f;
                    break;
            }
            rb.AddForce(new Vector2(Random.Range(1, 2 * power), Random.Range(1, power)), ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(0, -15 * power));

            gameObject.layer = LayerMask.NameToLayer("Decor");

            kicked = true;
        }
    }
}
