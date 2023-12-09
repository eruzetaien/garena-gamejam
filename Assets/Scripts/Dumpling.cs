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
    
    private int totalChicken = 0;
    private BoxCollider2D playerColl;
    private Rigidbody2D playerRb;
    
    private const  int MAX_CHICKEN_STATE_1 = 10;
    private const  int MAX_CHICKEN_STATE_2 = 20;
    private const  int MAX_CHICKEN_STATE_3 = 30;
    
    [SerializeField] private Slider chickenSlider1;
    [SerializeField] private Slider chickenSlider2;
    [SerializeField] private Slider chickenSlider3;


    [SerializeField] private Transform rightCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float rightCheckRadius = 0.1f;

    private Vector3 lastCheckpointPos;
    private bool active;

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

        lastCheckpointPos = transform.position;
        active = true;

        StartCoroutine(Tabrak());


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
    IEnumerator Tabrak()
    {
        active = false;
        IncreaseChickenBy(-10);
        playerRb.velocity = new Vector2(-5, 5);

        yield return new WaitForSeconds(1f);

        active = true;
        playerRb.velocity = Vector2.zero;
        playerRb.position = lastCheckpointPos;

        yield return new WaitUntil(() => OnTabrak());

        StartCoroutine(Tabrak());
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

        //RaycastHit2D hit = Physics2D.BoxCast(playerColl.bounds.center, playerColl.bounds.size, 0f, Vector2.down, distance, groundLayer);
        Collider2D a = Physics2D.OverlapCircle(groundCheck.position, distance, groundLayer);
        if (a != null) {
            return true;
        }
        return false;
    }

    bool OnTabrak()
    {
        if (Physics2D.OverlapCircle(rightCheck.position, rightCheckRadius, groundLayer) != null)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Chicken") && active)
        {
            Destroy(col.gameObject);
            IncreaseChickenBy(1);

        } else if (col.gameObject.CompareTag("Checkpoint"))
        {
            lastCheckpointPos = col.transform.position;

        }
    }

    private void IncreaseChickenBy(int amount)
    {
        totalChicken += amount;

        totalChicken = Mathf.Clamp(totalChicken, 0, MAX_CHICKEN_STATE_3);

        if (totalChicken > MAX_CHICKEN_STATE_2)
        {
            chickenState = ChickenState.STATE_3;
            transform.localScale = new Vector3(3f, 3f, 0);
            rightCheckRadius = 0.5f;
        }
        else if (totalChicken > MAX_CHICKEN_STATE_1)
        {
            chickenState = ChickenState.STATE_2;
            transform.localScale = new Vector3(2f, 2f, 0);
            rightCheckRadius = 0.3f;
        }
        else
        {
            chickenState = ChickenState.STATE_1;
            transform.localScale = new Vector3(1f, 1f, 0);
            rightCheckRadius = 0.1f;
        }

        chickenSlider1.value = totalChicken;
        chickenSlider2.value = totalChicken - 10;
        chickenSlider3.value = totalChicken - 20;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(rightCheck.position, rightCheckRadius);
    }
}
