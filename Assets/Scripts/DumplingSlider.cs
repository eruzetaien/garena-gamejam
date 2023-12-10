using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DumplingSlider : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider slider;
    [SerializeField] Sprite spriteRed, spriteNormal;
    [SerializeField] private Image icon;
    
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    public void SetValue(float value)
    {
        slider.value = value;
        if (value > 0)
        {
            icon.sprite = spriteRed;
        }
        else
        {
            icon.sprite = spriteNormal;
        }
    }

}
