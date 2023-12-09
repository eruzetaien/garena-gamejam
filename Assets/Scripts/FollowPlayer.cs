using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector3 offsetPosition;

    Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        offsetPosition = transform.position - player.position;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offsetPosition, 0.1f);
    }
}
