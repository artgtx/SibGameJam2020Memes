using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSwipe : MonoBehaviour
{
     private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;

    public float SWIPE_THRESHOLD = 20f;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            fingerUp =Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            fingerDown =Camera.main.ScreenToWorldPoint(Input.mousePosition);
            checkSwipe();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
           OnSwipeUp(); 
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnSwipeDown();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnSwipeRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnSwipeLeft();
        }

        /*
        foreach (Touch touch in Input.touches)
        {
            
            
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
            }
        }*/
    }

    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        //No Movement at-all
        else
        {
            //Debug.Log("No Swipe!");
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    void OnSwipeUp()
    {
        //Debug.Log("Swipe UP");
        FindObjectOfType<SpawnGrid>().movePlayer(SwipeType.up);
    }

    void OnSwipeDown()
    {
        //Debug.Log("Swipe Down");
        FindObjectOfType<SpawnGrid>().movePlayer(SwipeType.down);
    }

    void OnSwipeLeft()
    {
        //Debug.Log("Swipe Left");
        FindObjectOfType<SpawnGrid>().movePlayer(SwipeType.left);
    }

    void OnSwipeRight()
    {
        //Debug.Log("Swipe Right");
        FindObjectOfType<SpawnGrid>().movePlayer(SwipeType.rigth);
    }
    
    
    public enum SwipeType
    {
      up,down,left,rigth  
    }

  
}
