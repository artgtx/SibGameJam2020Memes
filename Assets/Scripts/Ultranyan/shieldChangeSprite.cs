using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldChangeSprite : MonoBehaviour
{
    [SerializeField] private Sprite active;
    [SerializeField] private Sprite unactive;
    [SerializeField] private SpriteRenderer sr;

    [SerializeField]private Master master;
    // Start is called before the first frame update

    private int coins;

    private void Start()
    {
        coins = 0;
        sr.sprite = unactive;
    }

    // Update is called once per frame
    void Update()
    {
        if (coins!=master._coins)
        {
            coins = master._coins;
            if (master._coins >= master.priceProtect)
            {
                sr.sprite = active;
            
            }
            else
            {
                sr.sprite = unactive;
            }
        }

       

    }
}
