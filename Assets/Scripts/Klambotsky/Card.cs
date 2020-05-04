using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;

    public Struct.CardType type_;
    public Struct.Gender gender_;
    public Struct.Media media_;
    float haterPower = 0;

    private float _speedAnimation = 0.1f;
    public Card(Struct.CardType type, Struct.Gender gender, Struct.Media media)
    {
        type_ = type;
        gender_ = gender;
        media_ = media;
    }

    public void SetCard(Struct.CardType type, Sprite sprite, Struct.Gender gender = Struct.Gender.Undefined, Struct.Media media = Struct.Media.Undefined)
    {
        type_ = type;
        gender_ = gender;
        media_ = media;
        sr.sprite = sprite;
    }

    public void SetCard(Struct.CardType type, Sprite[] sprites, Struct.Gender gender = Struct.Gender.Undefined, Struct.Media media = Struct.Media.Undefined)
    {
        type_ = type;
        gender_ = gender;
        media_ = media;

        StartCoroutine(FakeAnimation(sprites));
    }

    public IEnumerator FakeAnimation(Sprite[] sprites)
    {
        uint num = 0;
        int count = sprites.Length;
        while (true)
        {
            sr.sprite = sprites[num % count];
            num++;
            yield return new WaitForSeconds(_speedAnimation);
        }
    }
    public void OnBump()
    {
        blockIdle = true;
        switch (type_.type)
        {
            case Struct.CardType.Type.News:
                Master.instance.NewsScore(gender_, media_, GetInfo());
                break;
            case Struct.CardType.Type.Hater:
                Master.instance.HaterScore(gender_, haterPower);
                break;
            case Struct.CardType.Type.Coin:
                Master.instance.AddCoin();
                break;
            case Struct.CardType.Type.Protect:
                Master.instance.ProtectStore();
                break;
            case Struct.CardType.Type.Boost:
                Master.instance.BoostScore(gender_);
                break;
            case Struct.CardType.Type.Debuff:
                Master.instance.DebaffScore(gender_);
                break;
        }

        if (type_.type == Struct.CardType.Type.Coin)
        {
            gameSound.instance.onCoinSound();
        }
        else
        {
            gameSound.instance.onCardSound();
        }

        StartCoroutine(destroyCard());

    }

    private bool blockIdle = false;
    public void OnIdle()
    {
        if (!blockIdle)
        {
            switch (type_.type)
            {
                case Struct.CardType.Type.Hater:
                    haterPower += Master.instance.haterGrow;
                    print("Hater grows to " + haterPower);
                    break;
            }
        }

       
    }

    public string GetInfo()
    {
        return type_.type.ToString() + " " + gender_.ToString() + " " + media_.ToString();
    }
    public IEnumerator destroyCard()
    {
        float timeToMove = 0.1f;
        var currentPos =transform.localScale;
        var position = Vector3.zero;
        var t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.localScale = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        Destroy(transform.gameObject);
    }
    
    
    
    [SerializeField] private Sprite active;
    [SerializeField] private Sprite unactive;
    private Master master;
    // Start is called before the first frame update

    private int coins;

    private void Start()
    {
        if (type_.type == Struct.CardType.Type.Protect)
        { 
            master = FindObjectOfType<Master>();
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

    // Update is called once per frame
    void Update()
    {
        if (type_.type == Struct.CardType.Type.Protect)
        {
            if (coins != master._coins)
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

}
