using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Dumpling;

public class Human : MonoBehaviour
{
    [SerializeField] private SpriteRenderer glow;
    public enum HumanType
    {
        None = 0,
        Child,
        Adult,
        Chef
    }

    private Transform player;
    private bool isJump;
    
    public HumanType humanType = HumanType.None;
    Color defaultLightColor;

    void Start()
    {

        defaultLightColor = glow.color;

        if (humanType == HumanType.Chef)
        {
            if (SelfDestruct.player == null)
            {
                player = GameObject.FindWithTag("Player").transform;
            }
            else
            {
                player = SelfDestruct.player;
            }
        }

   

    }

    // Update is called once per frame
    void Update()
    {
        ChickenState chickenState = GameObject.FindWithTag("Player").GetComponent<Dumpling>().chickenState;
        switch (humanType)
        {
            case HumanType.Child:
                if (chickenState == ChickenState.STATE_1)
                {
                    // player kalah
                    glow.color = Color.red;
                }
                else
                {
                    // player menang
                    glow.color = defaultLightColor;
                }
                break;
            case HumanType.Adult:
                if (chickenState == ChickenState.STATE_1 ||
                    chickenState == ChickenState.STATE_2)
                {
                    // player kalah
                    glow.color = Color.red;
                }
                else
                {
                    // player menang
                    glow.color = defaultLightColor;
                }

                break;

            case HumanType.Chef:

                // player kalah
                glow.color = Color.red;

                break;
        }
    }
    
    // IEnumerator Jump(Vector2 endPosition)
    // {
    //     float timePased = 0f;
    //
    //     Vector2 startPosition = transform.position;
    //         
    //     float duration = 0.1f;
    //         
    //     while (timePased < duration)
    //     {
    //         timePased += Time.deltaTime;
    //         float linearT = timePased/ duration;
    //         transform.position = Vector2.Lerp(startPosition, endPosition, linearT);
    //
    //         yield return null;
    //     }
    // }
    //
    // void FixedUpdate()
    // {
    //     if (humanType != HumanType.Chef) return;
    //     
    //     if (isJump) return;
    //
    //     float distance = 5f;
    //     Vector2 direction = new Vector2(-0.5f,0f);
    //     
    //     RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, playerLayer);
    //     if (hit.collider != null)
    //     {
    //         isJump = true;
    //         StartCoroutine(Jump(player.position));
    //         Debug.Log("jump");
    //
    //     }
    //
    // }
    
    
}
