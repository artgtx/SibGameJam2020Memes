using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimationState : MonoBehaviour
{
    public Animator playerAnimator;
    // Start is called before the first frame update
    public void walk()
    {
        playerAnimator.SetBool("walk",true);
    }
    public void idle()
    {
        playerAnimator.SetBool("walk",false);
    }
}
