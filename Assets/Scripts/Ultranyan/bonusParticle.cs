using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonusParticle : MonoBehaviour
{
    float timeToMove = 0.2f;
    
    public GameObject bonus;

    public Sprite bonusSprite;
    public Sprite coinSprite;
    public Sprite negativeSprite;

    public Transform grandma;
    public Transform girl;
    public Transform man;

    public Transform coins;
    public Transform shild;


    void setBonus(Sprite bonusS, Vector3 startPos, Transform position)
    {
        GameObject tmp = Instantiate(bonus);
        tmp.transform.position = startPos;
        tmp.GetComponent<SpriteRenderer>().sprite = bonusS;
        StartCoroutine(MoveToPosition(tmp.transform, position.position));   
    }

    public void sendBonus(Struct.CardType type_,Struct.Gender gender_,Struct.Media media_,Vector3 startPos)
    {
        if (gender_ == Struct.Gender.Granny)
        {
            if (type_.type == Struct.CardType.Type.Debuff||type_.type ==Struct.CardType.Type.Hater)
            {
                setBonus(negativeSprite,startPos,grandma);
            }
            else
            {
                setBonus(bonusSprite,startPos,grandma);
            }

            
        }
        else if (gender_ == Struct.Gender.Man)
        {
            if (type_.type == Struct.CardType.Type.Debuff||type_.type ==Struct.CardType.Type.Hater)
            {
                setBonus(negativeSprite,startPos,man);
            }
            else
            {
                setBonus(bonusSprite,startPos,man);
            }
           
        }
        else if (gender_ == Struct.Gender.Woman)
        {
            
            if (type_.type == Struct.CardType.Type.Debuff||type_.type ==Struct.CardType.Type.Hater)
            {
                setBonus(negativeSprite,startPos,girl);
            }
            else
            {
                setBonus(bonusSprite,startPos,girl);
            }
        }
        else if (gender_ == Struct.Gender.Undefined&&type_.type==Struct.CardType.Type.Coin)
        {
            setBonus(coinSprite,startPos,coins);
        }
    }
    public IEnumerator MoveToPosition(Transform transform, Vector3 position)
    {
        var currentPos = transform.position;
        var t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        Destroy(transform.gameObject);
    }
}
