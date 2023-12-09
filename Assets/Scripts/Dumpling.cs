using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dumpling : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpPower = 9.0f;
    [SerializeField] private float fallPower = 50.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpriteRenderer playerSprite;

    
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

    [SerializeField] private float hungerDecreaseRate = 1;
    private float timer;

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
        ShowPlayer();
        
        IncreaseChickenBy(5);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = hungerDecreaseRate;
            IncreaseChickenBy(-1);
        }
    }

    private void FixedUpdate()
    {
        if (active)
        {
            RightMovement();
        
            Jump();
            Fall();
        }
    }
    IEnumerator Tabrak()
    {

        yield return new WaitUntil(() => OnTabrak());

        SoundManager.soundManager.Play("hurt");

        yield return new WaitUntil(() => OnTabrak());

        yield return StartCoroutine(Respawn(10));
        
        StartCoroutine(Tabrak());
    }

    IEnumerator Respawn(int damage)
    {
        active = false;
        IncreaseChickenBy(-damage);
        playerRb.velocity = Vector2.zero;
        playerRb.velocity = new Vector2(-5, 5);

        yield return new WaitForSeconds(1f);

        RefreshState();

        active = true;
        playerRb.velocity = Vector2.zero;
        playerRb.position = lastCheckpointPos;
    }

    private void RightMovement()
    {
        playerRb.velocity = new Vector2(speed, playerRb.velocity.y);
    }

    private void Jump()
    {
        if (OnGround() && (Input.GetKey(KeyCode.W)) )
        {
            SoundManager.soundManager.Play("jump");
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpPower);
        }
    }

    private void Fall()
    {
        if (OnGround() && Input.GetKey(KeyCode.S) )
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -fallPower);
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
            RefreshState();

        } else if (col.gameObject.CompareTag("Checkpoint"))
        {
            lastCheckpointPos = col.transform.position;

        } else if (col.gameObject.CompareTag("Human") && active)
        {
            Human human = col.gameObject.GetComponent<Human>();
            switch (human.humanType)
            {
                case Human.HumanType.Child :
                    if (chickenState == ChickenState.STATE_1)
                    {
                        StartCoroutine(Respawn(human.getDamage()));
                    }
                    else
                    {
                        Destroy(col.gameObject);
                    }
                    break;
                case Human.HumanType.Adult :
                    if (chickenState == ChickenState.STATE_1 ||
                        chickenState == ChickenState.STATE_2)
                    {
                        StartCoroutine(Respawn(human.getDamage()));
                    }
                    else
                    {
                        Destroy(col.gameObject);
                    }

                    break;

                case Human.HumanType.Chef :
                    StartCoroutine(ButtonRush());
                    break;
                default:
                    IncreaseChickenBy(-5);
                    break;
            }
        }
    }
    
    IEnumerator ButtonRush()
    {
        active = false;
        playerRb.velocity = Vector2.zero;
        float time = 3f;
        GameManager.Singleton.ActivateButtonRushPhase();
        yield return new WaitForSeconds(time);
        active = true;
        
        int pressedCount = GameManager.Singleton.DeactivateButtonRushPhase();
        if (pressedCount < 10)
        {
            yield return StartCoroutine(Respawn(10));
        }
        Time.timeScale = 1f;
    }

    private void HidePlayer()
    {
        Color transparentColor = playerSprite.color;
        transparentColor.a = 0f;
        playerSprite.color = transparentColor;
    }
    
    private void ShowPlayer()
    {
        Color transparentColor = playerSprite.color;
        transparentColor.a = 1f;
        playerSprite.color = transparentColor;
    }

    private void IncreaseChickenBy(int amount)
    {
        totalChicken += amount;

        totalChicken = Mathf.Clamp(totalChicken, 0, MAX_CHICKEN_STATE_3);
    }
    private void RefreshState()
    {
        if (totalChicken > MAX_CHICKEN_STATE_2)
        {
            chickenState = ChickenState.STATE_3;
            transform.localScale = new Vector3(3f, 3f, 0);
            rightCheckRadius = 0.7f;
        }
        else if (totalChicken > MAX_CHICKEN_STATE_1)
        {
            chickenState = ChickenState.STATE_2;
            transform.localScale = new Vector3(2f, 2f, 0);
            rightCheckRadius = 0.4f;
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
        
        if (totalChicken <= 0)
        {
            GameManager.Singleton.GameOver();
            HidePlayer();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(rightCheck.position, rightCheckRadius);
    }
}
