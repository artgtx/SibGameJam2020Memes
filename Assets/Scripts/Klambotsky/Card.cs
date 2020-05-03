using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite[] sprites;

    public Struct.CardType type_;
    Struct.Gender gender_;
    Struct.Media media_;
    float haterPower = 0;
    float haterGrow = 10;

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
        /*switch (type_.type)
        {
            case Struct.CardType.Type.News:
                sr.sprite = sprites[0];
                break;
            case Struct.CardType.Type.Hater:
                sr.sprite = sprites[1];
                break;
            case Struct.CardType.Type.Trap:
                sr.sprite = sprites[2];
                break;
            case Struct.CardType.Type.Coin:
                sr.sprite = sprites[3];
                break;
        }*/
    }

    public void OnBump()
    {
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
            case Struct.CardType.Type.Mix:
                Master.instance.MixCards();
                break;
        }
        
    }

    public void OnIdle()
    {
        switch (type_.type)
        {
            case Struct.CardType.Type.Hater:
                haterPower += Master.instance.haterGrow;
                print("Hater grows to " + haterPower);
                break;
        }
    }

    public string GetInfo()
    {
        return type_.type.ToString() + " " + gender_.ToString() + " " + media_.ToString();
    }
}
