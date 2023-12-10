using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cerita : MonoBehaviour
{
    public Sprite[] sprites;

    Image img;
    int count;

    void Start()
    {
        img = GetComponent<Image>();
        img.sprite = sprites[0];
        count = 0;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
        {
            count++;
            if (count < sprites.Length)
            {
                img.sprite = sprites[count];
            }
            else
            {
                SceneManager.LoadScene("Player");
            }
        }
        
        
    }
}
