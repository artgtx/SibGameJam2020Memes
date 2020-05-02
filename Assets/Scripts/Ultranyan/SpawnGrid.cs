using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGrid : MonoBehaviour
{
    public GameObject[] gridElements;
    private int playerPos;
    [SerializeField] private GameObject playerCard;
    
    
    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove,int cardposition)
    {
        Singletone.canMove = false;
        transform.gameObject.GetComponent<playerAnimationState>().walk();
        var currentPos = transform.position;
        var t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        spawncard(playerPos);
        playerPos = cardposition;
        transform.gameObject.GetComponent<playerAnimationState>().idle();
        Singletone.canMove = true;
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
        GridElement targetGridElement = gridElements[cardID].GetComponent<GridElement>();
        GridElement playerGridElement = gridElements[playerPos].GetComponent<GridElement>();
        Vector3 position = targetGridElement.currentCard.transform.position;
        Master.instance.DoStep();
        //вызов onBump
        targetGridElement.currentCard.GetComponent<Card>().OnBump();
        Destroy(targetGridElement.currentCard);
        
        targetGridElement.currentCard = playerGridElement.currentCard;
        
        foreach (var VARIABLE in FindObjectsOfType<Card>())
        {
            VARIABLE.OnIdle();
        }

        StartCoroutine(MoveToPosition(playerGridElement.currentCard.transform,position,0.3f,cardID));
        
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

    //public AnimationClip walk;
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

        //gridElements[playerPos].GetComponent<GridElement>().currentCard.GetComponent<Animation>().clip = walk;
        //gridElements[playerPos].GetComponent<GridElement>().currentCard.GetComponent<Animation>().Play;

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
    
    
}
