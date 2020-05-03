using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private static Generator _instance;
    public static Generator instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<Generator>();
            return _instance;
        }
    }

    [SerializeField] private GameObject card;
    [SerializeField] private bool test;
    // Start is called before the first frame update
    //start 1.9 1.9 dxdy = 1.9

    [SerializeField] private Sprite[] spritesTvGranny;
    [SerializeField] private Sprite[] spritesTvMan;
    [SerializeField] private Sprite[] spritesTvWoman;
    [SerializeField] private Sprite[] spritesYoutubeWoman;
    [SerializeField] private Sprite[] spritesYoutubeGranny;
    [SerializeField] private Sprite[] spritesYoutubeMan;
    [SerializeField] private Sprite[] spritesNewspaperWoman;
    [SerializeField] private Sprite[] spritesNewspaperGranny;
    [SerializeField] private Sprite[] spritesNewspaperMan;

    [SerializeField] private Sprite spriteHaterGranny;
    [SerializeField] private Sprite spriteHaterWoman;
    [SerializeField] private Sprite spriteHaterMan;

    [SerializeField] private Sprite coinSprite;
    [SerializeField] private Sprite protectSprite;
    [SerializeField] private Sprite mixSprite;

    [SerializeField] private Sprite exceptionSprite;

    void Start()
    {
        if (test)
        {

        }
    }

    //News	90%
    //Hater	10%

    public GameObject GenerateCard()
    {
        GameObject newCard = Instantiate(card);
        Card cardScript = newCard.GetComponent<Card>();

        int r = Random.Range(0, 100);
        int low = 0;
        for (int i = 1; i < Struct.cardTypes.Length; i++)
        {
            low += Struct.cardTypes[i - 1].probability;
            int high = Struct.cardTypes[i].probability + low;
            if (r >= low && r < high)
            {
                switch (Struct.cardTypes[i].type)
                {
                    case Struct.CardType.Type.News:
                        {
                            var gender = Struct.genders[Random.Range(0, Struct.genders.Length)];
                            var media = Struct.medias[Random.Range(0, Struct.medias.Length)];
                            var sprite = GetSpriteNew(gender, media);
                            cardScript.SetCard(Struct.cardTypes[i], sprite, gender, media);
                            return newCard;
                        }
                    case Struct.CardType.Type.Hater:
                        {
                            var gender = Struct.genders[Random.Range(0, Struct.genders.Length)];
                            var sprite = GetSpriteHater(gender);
                            cardScript.SetCard(Struct.cardTypes[i], sprite, gender);
                            return newCard;
                        }
                    case Struct.CardType.Type.Coin:
                        {
                            var sprite = coinSprite;
                            cardScript.SetCard(Struct.cardTypes[i], sprite);
                            return newCard;
                        }
                    case Struct.CardType.Type.Protect:
                        {
                            var sprite = protectSprite;
                            cardScript.SetCard(Struct.cardTypes[i], sprite);
                            return newCard;
                        }
                    case Struct.CardType.Type.Mix:
                        {
                            var sprite = mixSprite;
                            cardScript.SetCard(Struct.cardTypes[i], sprite);
                            return newCard;
                        }
                    default:
                        {
                            var sprite = exceptionSprite;
                            cardScript.SetCard(Struct.cardTypes[i], sprite);
                            return newCard;
                        }
                }
            }
        }
        throw new System.Exception("GenerateCard pick no card type");
    }

    private Sprite GetSpriteNew(Struct.Gender gender, Struct.Media media)
    {
        switch (gender)
        {
            case Struct.Gender.Granny:
                {
                    switch (media)
                    {
                        case Struct.Media.TV:
                            {
                                return spritesTvGranny[RandNum(spritesTvGranny.Length)];
                            }
                        case Struct.Media.Newspaper:
                            {
                                return spritesNewspaperGranny[RandNum(spritesNewspaperGranny.Length)];
                            }
                        case Struct.Media.Youtube:
                            {
                                return spritesYoutubeGranny[RandNum(spritesYoutubeGranny.Length)];
                            }
                    }
                    break;
                }
            case Struct.Gender.Woman:
                {
                    switch (media)
                    {
                        case Struct.Media.TV:
                            {
                                return spritesTvWoman[RandNum(spritesTvWoman.Length)];
                            }
                        case Struct.Media.Newspaper:
                            {
                                return spritesNewspaperWoman[RandNum(spritesNewspaperWoman.Length)];
                            }
                        case Struct.Media.Youtube:
                            {
                                return spritesYoutubeWoman[RandNum(spritesYoutubeWoman.Length)];
                            }
                    }
                    break;
                }
            case Struct.Gender.Man:
                {
                    switch (media)
                    {
                        case Struct.Media.TV:
                            {
                                return spritesTvMan[RandNum(spritesTvMan.Length)];
                            }
                        case Struct.Media.Newspaper:
                            {
                                return spritesNewspaperMan[RandNum(spritesNewspaperMan.Length)];
                            }
                        case Struct.Media.Youtube:
                            {
                                return spritesYoutubeMan[RandNum(spritesYoutubeMan.Length)];
                            }
                    }
                    break;
                }
        }

        return exceptionSprite;
    }

    private Sprite GetSpriteHater(Struct.Gender gender)
    {
        switch (gender)
        {
            case Struct.Gender.Granny:
            {
                return spriteHaterGranny;
            }
            case Struct.Gender.Woman:
            {
                return spriteHaterWoman;
            }
            case Struct.Gender.Man:
            {
                return spriteHaterMan;
            } 
        }

        return exceptionSprite;
    }

    private int RandNum(int length)
    {
        return Random.Range(0, length);
    }
    public Struct.Event GenerateEvent()
    {
        int r = Random.Range(0, 100);
        int low = 0;
        for (int i = 1; i < Struct.events.Length; i++)
        {
            low += Struct.events[i - 1].probability;
            int high = Struct.events[i].probability + low;
            if (r >= low && r < high)
            {
                return Struct.events[i];
            }
        }
        throw new System.Exception("GenerateEvent pick no events");
    }
}
