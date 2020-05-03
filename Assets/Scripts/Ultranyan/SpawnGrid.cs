using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGrid : MonoBehaviour
{
    public GameObject[] gridElements;
    private int playerPos;
    [SerializeField] private GameObject playerCard;
    
    
    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove,int cardposition,Struct.CardType type_)
    {
        transform.gameObject.GetComponent<playerAnimationState>().walk();
        var currentPos = transform.position;
        var t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        transform.gameObject.GetComponent<playerAnimationState>().idle();
        if (type_.type == Struct.CardType.Type.Mix)
        {
            playerPos = cardposition;
            respawnCards();
        }
        else
        {
            moveCards(cardposition,playerPos);
        }
        
    }


    public IEnumerator MoveToCardsCoroutine(int card,int cardTo,bool isLast)
    {
        
        float timeToMove = 0.3f;
        Transform transform=gridElements[card].GetComponent<GridElement>().currentCard.transform;
        Vector3 position=gridElements[cardTo].transform.position;
        gridElements[cardTo].GetComponent<GridElement>().currentCard = transform.gameObject;
        
        var currentPos = transform.position;
        var t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }

        if (isLast)
        {
            spawncard(card);
            Singletone.canMove = true;
        }

    }

    public void spawncard(int count)
    {
        GameObject generatedCard = Generator.instance.GenerateCard();
        generatedCard.transform.position = gridElements[count].transform.position;
        generatedCard.transform.parent = gridElements[count].transform;
       // generatedCard.transform.position=Vector3.zero;
        gridElements[count].GetComponent<GridElement>().currentCard = generatedCard ;
        
    }

    public void replaceCards(int cardID)
    {
        Singletone.canMove = false;
        GridElement targetGridElement = gridElements[cardID].GetComponent<GridElement>();
        GridElement playerGridElement = gridElements[playerPos].GetComponent<GridElement>();
        Vector3 position = targetGridElement.currentCard.transform.position;
        Master.instance.DoStep();
        //вызов onBump
        targetGridElement.currentCard.GetComponent<Card>().OnBump();
        Struct.CardType type_=targetGridElement.currentCard.GetComponent<Card>().type_;
        Destroy(targetGridElement.currentCard);
        
        
        targetGridElement.currentCard = playerGridElement.currentCard;
        playerGridElement.currentCard = null;
        
        foreach (var VARIABLE in FindObjectsOfType<Card>())
        {
            VARIABLE.OnIdle();
        }

        StartCoroutine(MoveToPosition(targetGridElement.currentCard.transform,position,0.3f,cardID,type_));
        
    }

    public void movePlayer(CheckSwipe.SwipeType typeMove)
    {
        if (Singletone.canMove)
        {
            switch (typeMove)
        {
         case CheckSwipe.SwipeType.up:
             if (playerPos - 3 >= 0)
             {
                 replaceCards(playerPos - 3 );
             }

             break;
         case CheckSwipe.SwipeType.down:
             if (playerPos + 3 < 9)
             {
                 replaceCards(playerPos + 3 );
             }
             break;
         case CheckSwipe.SwipeType.left :
             if (playerPos - 1 >= 0&&playerPos!=3&&playerPos!=6)
             {
                 replaceCards(playerPos - 1 );
             }
             break;
         case CheckSwipe.SwipeType.rigth:
             if (playerPos + 1 < 9&&playerPos!=2&&playerPos!=5)
             {
                 replaceCards(playerPos + 1 );
             }
             break;
        }
        }
       
    }

    public void destoyAllCards()
    {
        for (int i = 0; i < 9; i++)
        {
            if (i != playerPos)
            {
                print(playerPos);
                Destroy(gridElements[i].GetComponent<GridElement>().currentCard); 
            }
        }
    }

    public void respawnCards()
    {
        destoyAllCards();
        for (int i = 0; i < 9; i++)
        {
            if(i!=playerPos)
            {
                spawncard(i);
            }
        }

       StartCoroutine("startAnimation");

    }

    public void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            if(i!=4)
            {
               spawncard(i);
            }
            else
            {
                playerPos = i;
                gridElements[i].GetComponent<GridElement>().setCard(Instantiate(playerCard, gridElements[i].GetComponent<Transform>()));
            }
        }

        StartCoroutine("startAnimation");
    }

    IEnumerator startAnimation()
    {
        Singletone.canMove = false;
        int rotation = 90;
        int rotationAngle = 5;
        
        foreach (GameObject gridElementGO in gridElements)
        {
           gridElementGO.GetComponent<GridElement>().currentCard.transform.Rotate(new Vector3(0,rotation,0));
        }

        while (rotation!=0)
        {
            yield return new WaitForSeconds(0.03f);
            rotation -= rotationAngle;
            foreach (GameObject gridElementGO in gridElements)
            {
                gridElementGO.GetComponent<GridElement>().currentCard.transform.Rotate(new Vector3(0,-rotationAngle,0));
            }

        }

        Singletone.canMove = true;

    }
    
        public void moveCards(int newPosition, int oldPosition)
    {
        playerPos = newPosition;
        if (Singletone.calculateColumnsRovs(newPosition)[0] != Singletone.calculateColumnsRovs(oldPosition)[0])
        {
            if (Singletone.calculateColumnsRovs(oldPosition)[0] == 2 && Singletone.calculateColumnsRovs(newPosition)[0] == 3)
            {
                if (Singletone.calculateColumnsRovs(oldPosition)[1] == 1)
                {
                    StartCoroutine(MoveToCardsCoroutine(0,oldPosition,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[1] == 2)
                {
                    StartCoroutine(MoveToCardsCoroutine(1,oldPosition,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[1] == 3)
                {
                    StartCoroutine(MoveToCardsCoroutine(2,oldPosition,true));
                }
            }
            
            else if (Singletone.calculateColumnsRovs(oldPosition)[0] == 2 && Singletone.calculateColumnsRovs(newPosition)[0] == 1)
            {
                if (Singletone.calculateColumnsRovs(oldPosition)[1] == 1)
                {
                    StartCoroutine(MoveToCardsCoroutine(6,oldPosition,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[1] == 2)
                {
                    StartCoroutine(MoveToCardsCoroutine(7,oldPosition,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[1] == 3)
                {
                    StartCoroutine(MoveToCardsCoroutine(8,oldPosition,true));
                }
            }
            else if (Singletone.calculateColumnsRovs(oldPosition)[0] == 1 && Singletone.calculateColumnsRovs(newPosition)[0] == 2)
            {
                if (Singletone.calculateColumnsRovs(oldPosition)[1] == 1)
                {
                    StartCoroutine(MoveToCardsCoroutine(1,oldPosition,false));
                    StartCoroutine(MoveToCardsCoroutine(2,1,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[1] == 2)
                {
                    StartCoroutine(MoveToCardsCoroutine(2,oldPosition,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[1] == 3)
                {
                    StartCoroutine(MoveToCardsCoroutine(1,oldPosition,false));
                    StartCoroutine(MoveToCardsCoroutine(0,1,true));
                }
            }
            else if (Singletone.calculateColumnsRovs(oldPosition)[0] == 3 && Singletone.calculateColumnsRovs(newPosition)[0] == 2)
            {
                if (Singletone.calculateColumnsRovs(oldPosition)[1] == 1)
                {
                    StartCoroutine(MoveToCardsCoroutine(7,oldPosition,false));
                    StartCoroutine(MoveToCardsCoroutine(8,7,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[1] == 2)
                {
                    StartCoroutine(MoveToCardsCoroutine(8,oldPosition,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[1] == 3)
                {
                    StartCoroutine(MoveToCardsCoroutine(7,oldPosition,false));
                    StartCoroutine(MoveToCardsCoroutine(6,7,true));
                }
            }

        }

        else if (Singletone.calculateColumnsRovs(newPosition)[1] != Singletone.calculateColumnsRovs(oldPosition)[1])
        {
            if (Singletone.calculateColumnsRovs(oldPosition)[1] == 2 && Singletone.calculateColumnsRovs(newPosition)[1] == 1)
            {
                if (Singletone.calculateColumnsRovs(oldPosition)[0]==1)
                {
                    StartCoroutine(MoveToCardsCoroutine(2,oldPosition,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[0]==2)
                {
                    StartCoroutine(MoveToCardsCoroutine(5,oldPosition,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[0]==3)
                {
                    StartCoroutine(MoveToCardsCoroutine(8,oldPosition,true));
                }
            }

            else if (Singletone.calculateColumnsRovs(oldPosition)[1] == 2 && Singletone.calculateColumnsRovs(newPosition)[1] == 3)
            {
                if (Singletone.calculateColumnsRovs(oldPosition)[0]==1)
                {
                    StartCoroutine(MoveToCardsCoroutine(0,oldPosition,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[0]==2)
                {
                    StartCoroutine(MoveToCardsCoroutine(3,oldPosition,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[0]==3)
                {
                    StartCoroutine(MoveToCardsCoroutine(6,oldPosition,true));
                }
            }
            
            else if (Singletone.calculateColumnsRovs(oldPosition)[1] == 3 && Singletone.calculateColumnsRovs(newPosition)[1] == 2)
            {
                if (Singletone.calculateColumnsRovs(oldPosition)[0]==1)
                {
                    StartCoroutine(MoveToCardsCoroutine(5,oldPosition,false));
                    StartCoroutine(MoveToCardsCoroutine(8,5,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[0]==2)
                {
                    StartCoroutine(MoveToCardsCoroutine(2,oldPosition,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[0]==3)
                {
                    StartCoroutine(MoveToCardsCoroutine(5,oldPosition,false));
                    StartCoroutine(MoveToCardsCoroutine(2,5,true));
                }
            }
            else if (Singletone.calculateColumnsRovs(oldPosition)[1] == 1 && Singletone.calculateColumnsRovs(newPosition)[1] == 2)
            {
                if (Singletone.calculateColumnsRovs(oldPosition)[0]==1)
                {
                    StartCoroutine(MoveToCardsCoroutine(3,oldPosition,false));
                    StartCoroutine(MoveToCardsCoroutine(6,3,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[0]==2)
                {
                    StartCoroutine(MoveToCardsCoroutine(0,oldPosition,true));
                }
                else if (Singletone.calculateColumnsRovs(oldPosition)[0]==3)
                {
                    StartCoroutine(MoveToCardsCoroutine(3,oldPosition,false));
                    StartCoroutine(MoveToCardsCoroutine(0,3,true));
                }
            }

        }
    }

    
    public void MixCards()
    {

    }
}
