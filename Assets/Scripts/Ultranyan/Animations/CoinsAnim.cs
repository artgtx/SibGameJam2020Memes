using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsAnim : MonoBehaviour
{
    public Animator anim;
    public Text coinText;
    private string text;

    private void Start()
    {
        text = coinText.text;
    }

    // Update is called once per frame
    void Update()
    {
        //никогда такого не делайте, только если сейчас 5 часов утра и скоро сдавать работу)))
        
        if (text != coinText.text)
        {
            anim.Play("coin");
        }
    }
}
