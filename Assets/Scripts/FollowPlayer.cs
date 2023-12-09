using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector3 offsetPosition;
    public float smoothTime = 0.25f;

    Transform player;
    Vector3 velocity;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        offsetPosition = transform.position - player.position;
    }

    void LateUpdate()
    {
        transform.position = player.position + offsetPosition;
        //transform.position = Vector3.SmoothDamp(transform.position, player.position + offsetPosition, ref velocity, smoothTime);
        //transform.position = Vector3.Lerp(transform.position, player.transform.position + offsetPosition, 0.1f);
    }
}
