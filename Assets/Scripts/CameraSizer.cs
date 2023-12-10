using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Dumpling;
using static Human;

public class CameraSizer : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 20;

    private Dumpling dumpling;
    private Camera cam;

    private float targetFov;
    GameManager gm;

    void Start()
    {
        cam = GetComponent<Camera>();
        dumpling = FindObjectOfType<Dumpling>();
        gm = FindAnyObjectByType<GameManager>();
    }


    void Update()
    {
        if (gm.isButtonRushActive)
        {
            targetFov = 50;
        }
        else
        {
            ChickenState chickenState = dumpling.chickenState;
            if (chickenState == ChickenState.STATE_1)
            {
                // state 1
                targetFov = 50;
            }
            else if (chickenState == ChickenState.STATE_2)
            {
                // state 2
                targetFov = 60;
            }
            else
            {
                // state 3
                targetFov = 70;
            }

        }
        cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, targetFov, zoomSpeed * Time.deltaTime);

    }
}
