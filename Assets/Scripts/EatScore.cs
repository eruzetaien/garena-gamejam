using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EatScore : MonoBehaviour
{
    public void Init(string Score, Vector2 spawnOffset, bool isText =false)
    {
        transform.position = (Vector2)transform.position + spawnOffset;
        TextMeshPro textMeshPro = GetComponent<TextMeshPro>();   
        if (isText)
        {
            textMeshPro.text = "<color=#ff8a04>" + Score + "</color>";
            textMeshPro.fontSize = 15f;
            transform.position = transform.position + new Vector3(0, 1f, 0);
        }
        else
        {
            textMeshPro.text = Score.ToString();
        }
        Destroy(gameObject, 5f);
    }
}
