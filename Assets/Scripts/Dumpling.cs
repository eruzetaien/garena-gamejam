using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Dumpling : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpPower = 9.0f;
    [SerializeField] private float fallPower = 50.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private GameObject eatScorePrefab;
    [SerializeField] private GameObject explosionPrefab;


    private int totalChicken = 0;
    private CircleCollider2D playerColl;
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

    // Animation
    private Animator animator;
    [SerializeField] private AnimatorController phase1Controller;
    [SerializeField] private AnimatorOverrideController phase2Controller;
    [SerializeField] private AnimatorOverrideController phase3Controller;

    // Cam shake
    private Animator camAnim;

    private bool isGrounded;


    // eating animation
    private float lengthAfterEat = 0.1f;
    private float eatTimer;

    public enum ChickenState
    {
        STATE_1,
        STATE_2,
        STATE_3
    }
    
    public ChickenState chickenState = ChickenState.STATE_1;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerColl = GetComponent<CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();
        camAnim = Camera.main.GetComponent<Animator>();

        lastCheckpointPos = transform.position;
        active = true;

        StartCoroutine(Tabrak());
        ShowPlayer();
        
        IncreaseChickenBy(9);
    }

    // Update is called once per frame
    void Update()
    {
        // decrease hunger
        timer -= Time.deltaTime;
        if (timer < 0 && active && 
            (totalChicken != MAX_CHICKEN_STATE_1 + 1 || totalChicken != MAX_CHICKEN_STATE_2 + 1 ) 
            )
        {
            timer = hungerDecreaseRate;
            IncreaseChickenBy(-1);
            RefreshState();
        }
        animator.SetFloat("yVelocity", playerRb.velocity.y);


        // eating animation
        eatTimer -= Time.deltaTime;
        if (eatTimer < 0)
        {
            animator.SetBool("isEating", false);
        }
        else
        {
            if (!animator.GetBool("isEating"))
            {
                animator.Play("Phase1_eat");
            }
            animator.SetBool("isEating", true);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = OnGround();
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

        animator.SetTrigger("hurt");

        SoundManager.soundManager.Play("hurt");


        yield return StartCoroutine(Respawn(10, true));
        
        StartCoroutine(Tabrak());
    }

    IEnumerator Respawn(int damage, bool onCheckPoint)
    {
        active = false;
        IncreaseChickenBy(-damage);
        camAnim.SetTrigger("shake");

        playerRb.velocity = Vector2.zero;
        if (onCheckPoint)
        {
            playerRb.velocity = new Vector2(-5, 5);
            yield return new WaitForSeconds(0.5f);
        }
        

        // respawn
        RefreshState();

        if (! onCheckPoint)
        {
            playerRb.velocity = new Vector2(10, 5);
            yield return new WaitForSeconds(0.5f);
        }


        playerRb.velocity = Vector2.zero;

        animator.SetTrigger("respawn");
    
        if (onCheckPoint)
        {
            playerRb.position = lastCheckpointPos;
        }
        active = true;

    }

    private void RightMovement()
    {
        playerRb.velocity = new Vector2(speed, playerRb.velocity.y);
    }

    private void Jump()
    {
        if (isGrounded && (Input.GetKey(KeyCode.W)) )
        {
            SoundManager.soundManager.Play("jump");
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpPower);
        }
    }

    private void Fall()
    {
        if (!isGrounded && Input.GetKey(KeyCode.S) )
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -fallPower);
        }
    }
    
    bool OnGround() {
        float distance = 0.1f;

        //RaycastHit2D hit = Physics2D.BoxCast(playerColl.bounds.center, playerColl.bounds.size, 0f, Vector2.down, distance, groundLayer);
        Collider2D a = Physics2D.OverlapCircle(groundCheck.position, distance, groundLayer);
        
        if (a != null) {
            if (!isGrounded){
                switch (chickenState)
                {
                    case ChickenState.STATE_1:
                        camAnim.SetTrigger("lightShake");
                        break;
                    case ChickenState.STATE_2:
                        camAnim.SetTrigger("shake");
                        break;
                    case ChickenState.STATE_3:
                        camAnim.SetTrigger("heavyShake");
                        break;

                }
                SoundManager.soundManager.Play("land");
            }
            animator.SetBool("isGrounded", true);
            return true;
        }
        animator.SetBool("isGrounded", false);
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

    private void BiteSfx()
    {
        switch (chickenState)
        {
            case ChickenState.STATE_1:
                eatTimer = lengthAfterEat;
                SoundManager.soundManager.Play("bite1");
                break;
            case ChickenState.STATE_2:
                eatTimer = lengthAfterEat;
                SoundManager.soundManager.Play("bite2");
                break;
            case ChickenState.STATE_3:
                eatTimer = lengthAfterEat;
                SoundManager.soundManager.Play("bite3");
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Chicken") && active)
        {
            BiteSfx();
            Destroy(col.gameObject);
            IncreaseChickenBy(1);
            RefreshState();
            GameManager.Singleton.AddScore(1);
            SpawnScore("1");
        

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
                        StartCoroutine(Respawn(10,false));
                    }
                    else
                    {
                        Destroy(col.gameObject);
                        GameManager.Singleton.AddScore(50);
                        SpawnScore("50");
                    }
                    break;
                case Human.HumanType.Adult :
                    if (chickenState == ChickenState.STATE_1 ||
                        chickenState == ChickenState.STATE_2)
                    {
                        StartCoroutine(Respawn(10,false));
                    }
                    else
                    {
                        BiteSfx();
                        Destroy(col.gameObject);
                        GameManager.Singleton.AddScore(100);
                        SpawnScore("100");
                    }

                    break;

                case Human.HumanType.Chef :
                    StartCoroutine(ButtonRush(col.gameObject));
                    break;
                default:
                    IncreaseChickenBy(-5);
                    break;
            }
        }
    }
    
    IEnumerator ButtonRush( GameObject chef)
    {
        active = false;
        playerRb.velocity = Vector2.zero;
        float duration = 3f;
        float time = 0;
        
        GameManager.Singleton.ActivateButtonRushPhase();
        bool isButtonRushSuccess = false;
        
        while (time < duration && !GameManager.Singleton.IsButtonRushSuccess() )
        {
            time += Time.deltaTime;
            yield return null;
        }
        isButtonRushSuccess = GameManager.Singleton.IsButtonRushSuccess();
        
        if (isButtonRushSuccess)
        {
            GameManager.Singleton.PlaySuccessAnimation();
            yield return new WaitForSeconds(1f);
            Instantiate(explosionPrefab, chef.transform.position, Quaternion.identity);
            GameManager.Singleton.DeactivateButtonRushPhase();
            SoundManager.soundManager.Play("explosion");
            Destroy(chef);
            GameManager.Singleton.AddScore(150);
            SpawnScore("150");
            SpawnScore("NICEEE", true);
            active = true;
            Time.timeScale = 1f;
        }
        else
        {
            GameManager.Singleton.DeactivateButtonRushPhase();
            SoundManager.soundManager.Play("explosion");
            yield return StartCoroutine(Respawn(10,false));
            active = true;
            Time.timeScale = 1f;
        }

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

            animator.runtimeAnimatorController = phase3Controller;
            rightCheck.localPosition = new Vector3(1.5f, 1, 0);
            playerColl.radius = 1.5f;
            playerColl.offset = new Vector2(0, 1);


            //transform.localScale = new Vector3(3f, 3f, 0);

            rightCheckRadius = 0.7f;
        }
        else if (totalChicken > MAX_CHICKEN_STATE_1)
        {
            chickenState = ChickenState.STATE_2;

            animator.runtimeAnimatorController = phase2Controller;
            rightCheck.localPosition = new Vector3(1, 0.5f, 0);
            playerColl.radius = 1f;
            playerColl.offset = new Vector2(0, 0.5f);




            //transform.localScale = new Vector3(2f, 2f, 0);
            rightCheckRadius = 0.4f;
        }
        else
        {
            chickenState = ChickenState.STATE_1;

            animator.runtimeAnimatorController = phase1Controller;
            rightCheck.localPosition = new Vector3(0.5f, 0, 0);
            playerColl.radius = 0.5f;
            playerColl.offset = new Vector2(0, 0);




            //transform.localScale = new Vector3(1f, 1f, 0);
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

    private void SpawnScore(string score,bool isText = false)
    {
        float randomXoffset = Random.Range(-0.5f, 1f);
        float randomYoffset = Random.Range(1.5f, 2.5f);

        Vector3 spawnOffset = new Vector3(randomXoffset, randomYoffset, 0);

        GameObject eatScoreGO = Instantiate(eatScorePrefab, transform);
        eatScoreGO.GetComponent<EatScore>().Init(score,spawnOffset,isText);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(rightCheck.position, rightCheckRadius);
    }
}
