using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonRush : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pressCountText;
    
    private int pressCount = 0;
    private bool isActive = true;
    
    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.Space) )
            {
                pressCount++;
                pressCountText.text = pressCount.ToString();
            }
        }
    }

}
