using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EatScore : MonoBehaviour
{
    public void Init(int Score, Vector2 spawnOffset)
    {
        transform.position = (Vector2)transform.position + spawnOffset;
        GetComponent<TextMeshPro>().text = Score.ToString();
        Destroy(gameObject, 5f);
    }
}
