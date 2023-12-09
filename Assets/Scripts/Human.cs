using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] private int damage;

    public enum HumanType
    {
        None = 0,
        Child,
        Adult,
        Chef
    }

    public HumanType humanType = HumanType.None;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getDamage()
    {
        return damage;
    }
}
