using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;

public class StatusBarController : MonoBehaviour
{
    [SerializeField] private GameObject progressObject;
    public typeCategory category;
    private float _maxProgress;
    private float _progress;
    public float _needProgress;

    private float _persent = 50;
    public float _needPersent;

    private float _x;

    private float _shift = 0.001f;
    private float _waitTime = 0.001f;
    //private float _waitTimeBliking = 0.1f;

    public float _startProgress = 0.0f;

    private object _locker = new object();

    //private Color _currentColor;

    [SerializeField] private Sprite spriteBad;
    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteGood;
    void Awake()
    {
        _maxProgress = progressObject.transform.localScale.y;
        _x = progressObject.transform.localScale.x;

        progressObject.transform.localScale = new Vector3(_x, _startProgress);
        _progress = _startProgress;
    }

    public void ChangeStatus(float persent)
    {
        _needProgress = _maxProgress / 100 * persent;
        _needPersent = persent;

        lock (_locker)
        {
            StartCoroutine(ChangeProgressCoroutine());
            //StartCoroutine(ChangeColorCoroutine(GetCountBlink(persent)));
        }
    }

    /*private int GetCountBlink(float persent)
    {
        float shift = Math.Abs(persent);
        if(shift < 5)
        {
            return 1;
        }
        if(shift > 5 && shift < 10)
        {
            return 2;
        }
        return 3;
    }*/

    IEnumerator ChangeProgressCoroutine()
    {
        while (Math.Abs(_progress - _needProgress) >= _shift && _progress > (0.0 - _shift) && _progress < (_maxProgress + _shift))
        {
            if (_progress > _needProgress)
            {
                _progress -= _shift;
            }
            else
            {
                _progress += _shift;
            }

            if(_progress > _maxProgress)
            {
                _progress = _maxProgress;
            }
            
            if(_progress < 0.0)
            {
                _progress = 0;
            }

            ChangeColor();

            progressObject.transform.localScale = new Vector3(_x, _progress);
            yield return new WaitForSeconds(_waitTime);
        }

        //progressObject.GetComponent<SpriteRenderer>().color = _currentColor;

        _persent = _needPersent;
    }

    private void ChangeColor()
    {
        float persent = _progress * 100 / _maxProgress;
        if(persent <= 20)
        {
            progressObject.GetComponent<SpriteRenderer>().sprite = spriteBad;
            return;
        }
        if(persent > 20 && persent < 70)
        {
            progressObject.GetComponent<SpriteRenderer>().sprite = spriteNormal;
            return;
        }
        if (persent >= 70)
        {
            progressObject.GetComponent<SpriteRenderer>().sprite = spriteGood;
        }
    }

    /*IEnumerator ChangeColorCoroutine(int count)
    {
        Color colorProgress = progressObject.GetComponent<SpriteRenderer>().color;

        Color color = _currentColor;
        colorProgress.a = 0;
        progressObject.GetComponent<SpriteRenderer>().color = colorProgress;

        yield return new WaitForSeconds(_waitTimeBliking);

        bool isBlink = true;
        for(int i=0; i<count; i++)
        {
            if (isBlink)
            {
                color.a = 100;
                isBlink = false;
            }
            else
            {
                color.a = 0;
                isBlink = true;
            }
            progressObject.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(_waitTimeBliking);
        }
    }
    */

    public enum typeCategory
    {
        granny, man, woman
    }
}
