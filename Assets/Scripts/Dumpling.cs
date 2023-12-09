using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dumpling : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpPower = 9.0f;
    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private int totalChicken = 0;
    private BoxCollider2D playerColl;
    private Rigidbody2D playerRb;
    
    private const  int MAX_CHICKEN_STATE_1 = 10;
    private const  int MAX_CHICKEN_STATE_2 = 20;
    private const  int MAX_CHICKEN_STATE_3 = 30;
    
    [SerializeField] private Slider chickenSlider1;
    [SerializeField] private Slider chickenSlider2;
    [SerializeField] private Slider chickenSlider3;

    enum ChickenState
    {
        STATE_1,
        STATE_2,
        STATE_3

    }
    
    private ChickenState chickenState = ChickenState.STATE_1;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerColl = GetComponent<BoxCollider2D>();
        
                
        totalChicken = 13;
        
        UpdateState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Jump();
        RightMovement();
    }

    private void RightMovement()
    {
        playerRb.velocity = new Vector2(speed, playerRb.velocity.y);
    }

    private void Jump()
    {
        if (OnGround() && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) )
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpPower);
        }
    }
    
    bool OnGround() {
        float distance = 0.1f;

        RaycastHit2D hit = Physics2D.BoxCast(playerColl.bounds.center, playerColl.bounds.size, 0f, Vector2.down, distance, groundLayer);
        if (hit.collider != null) {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Chicken"))
        {
            Destroy(col.gameObject);
            EatChicken();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Human"))
        {
          TakeDamage(col.gameObject.GetComponent<Human>().getDamage());  
        }

    }
    
    private void TakeDamage(int damage)
    {
         totalChicken -= damage;
         
         if (totalChicken < 0)
         {
             totalChicken = 0;
         }
         
         UpdateState();
    }

    private void EatChicken()
    {
        if (totalChicken == MAX_CHICKEN_STATE_3){return;}
        
        totalChicken++;
        
        UpdateState();
    }

    private void UpdateState()
    {
        if (totalChicken > MAX_CHICKEN_STATE_2)
        {
            chickenState = ChickenState.STATE_3;
            transform.localScale = new Vector3(2f,2f,0);
        }
        else if (totalChicken > MAX_CHICKEN_STATE_1)
        {
            chickenState = ChickenState.STATE_2;
            transform.localScale = new Vector3(1.5f,1.5f,0);
        }
        else
        {
            chickenState = ChickenState.STATE_1;
            transform.localScale = new Vector3(1f,1f,0);
        }

        chickenSlider1.value = totalChicken ;
        chickenSlider2.value = totalChicken - 10;
        chickenSlider3.value = totalChicken - 20;
    }
}