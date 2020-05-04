using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectController : MonoBehaviour
{
    [SerializeField] private GameObject[] valueProtectObjects;

    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    private int _countActive = 0;
    [SerializeField] private int maxCountActive = 5;
    public void AddValue(int count)
    {
        for(int countAdd = 0; countAdd < count; countAdd++)
        {
            if(_countActive < maxCountActive)
            {
                valueProtectObjects[_countActive].GetComponent<SpriteRenderer>().sprite = activeSprite;
                _countActive++;
            }
            else
            {
                break;
            }
        }
    }

    public void SubValue(int count)
    {
        for (int countAdd = 0; countAdd < count; countAdd++)
        {
            if (_countActive > 0)
            {
                valueProtectObjects[_countActive-1].GetComponent<SpriteRenderer>().sprite = inactiveSprite;
                _countActive--;
            }
            else
            {
                break;
            }
        }
    }
}
