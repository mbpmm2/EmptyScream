using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Config")]
    public float maxHealth;
    public float maxSanity;

    [Header("Player Current State")]
    public float health;
    public float sanity;

    //[Header("Components Assigment")]
    //Player Inventory;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        sanity = maxSanity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
