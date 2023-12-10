using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowXCam : MonoBehaviour
{
    Transform cam;

    void Start()
    {
        cam = Camera.main.transform.parent;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(cam.position.x, transform.position.y, 10);
    }
}
