using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameSound : MonoBehaviour
{
    public AudioSource bgMusic;
    public AudioSource effects;
    
    public AudioClip gameTheme;

    public AudioClip gameOver;

    public AudioClip win;

    public AudioClip coin;

    public AudioClip card;
    
    public AudioClip btn;
    // Start is called before the first frame update
    private static gameSound _instance;
    public static gameSound instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<gameSound>();
            return _instance;
        }
    }
    
    void Start()
    {
        bgMusic.clip = gameTheme;
        bgMusic.Play();
    }

    public void onLoseSound()
    {
        bgMusic.Stop();
        bgMusic.PlayOneShot(gameOver);
    }
    public void onWinSound()
    {
        bgMusic.Stop();
        bgMusic.PlayOneShot(win);
    }
    public void onCoinSound()
    {
        effects.PlayOneShot(coin);
    }

    public void onCardSound()
    {
        effects.PlayOneShot(card);
    }

    public void onBtnPsrSound()
    {
        effects.PlayOneShot(btn);
    }
}
