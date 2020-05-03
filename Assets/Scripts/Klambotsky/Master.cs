using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Master : MonoBehaviour
{
    public StatusBarController grannyStatusBar;
    public StatusBarController womanStatusBar;
    public StatusBarController manStatusBar;

    [SerializeField] private Text coinsText;
    [SerializeField] private Text stepsProtectText;

    private float maxCount = 1000;
    private static Master _instance;
    public static Master instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<Master>();
            return _instance;
        }
    }

    //Balance info
    [SerializeField] string Delta_INFO;
    public float tvDelta = 10; // сколько дает телевизор
    public float NewspaperDelta = 20; // сколько дает интернет
    public float youtubeDelta = 30; //сколько дает ютуб
    public float downDelta = 10; //сколько отнимается у остальных
    public float haterGrow = 10; //насколько быстро растет хейтер

    [SerializeField] string Multiplier_INFO;
    public float womanMultiplier = 1; //инфо о множителях
    public float manMultiplier = 1; //
    public float grannyMultiplier = 1;

    // PlayerScore
    private int startScore = 200;
    public float womanScore;
    public float manScore;
    public float grannyScore;

    private int _coins = 0;

    private int _currentStepsProtect;
    public int stepsProtect = 5;
    public int protect = 5;
    public int priceProtect = 2;

    private void Start()
    {
        womanScore = startScore;
        manScore = startScore;
        grannyScore = startScore;
        UpdateStatusBar();
    }

    public bool CheckDefeat() => womanScore <= 0 || manScore <= 0 || grannyScore <= 0;

    private void UpdateStatusBar()
    {
        grannyStatusBar.ChangeStatus(grannyScore / maxCount * 100);
        womanStatusBar.ChangeStatus(womanScore / maxCount * 100);
        manStatusBar.ChangeStatus(manScore / maxCount * 100);
    }

    private void PreventNegative()
    {
        if (grannyScore < 0)
        {
            grannyScore = 0;
        }
        if (womanScore < 0)
        {
            womanScore = 0;
        }
        if (manScore < 0)
        {
            manScore = 0;
        }
    }

    public void ShowEvent()
    {
        Struct.Event currentEvent = Generator.instance.GenerateEvent();
        Debug.Log(currentEvent.name);
        womanMultiplier = currentEvent.GetWomanMultiplier();
        manMultiplier = currentEvent.GetManMultiplier();
        grannyMultiplier = currentEvent.GetGrannyMultiplier();
    }

    public void HaterScore(Struct.Gender gender, float value)
    {
        if(_currentStepsProtect > 0)
        {
            value += protect;
        }

        switch (gender)
        {
            case Struct.Gender.Undefined:
                Debug.Log("Card gender is Undefined");
                break;
            case Struct.Gender.Woman:
                womanScore -= value;
                break;
            case Struct.Gender.Man:
                manScore -= value;
                break;
            case Struct.Gender.Granny:
                grannyScore -= value;
                break;
        }
        PreventNegative();
        UpdateStatusBar();
    }

    public void NewsScore(Struct.Gender gender, Struct.Media media, string info)
    {
        float upDelta = 0;
        switch (media)
        {
            case Struct.Media.Undefined:
                Debug.Log("Card media is Undefined");
                break;
            case Struct.Media.TV:
                upDelta = tvDelta;
                break;
            case Struct.Media.Newspaper:
                upDelta = NewspaperDelta;
                break;
            case Struct.Media.Youtube:
                upDelta = youtubeDelta;
                break;
        }

        switch (gender)
        {
            case Struct.Gender.Undefined:
                Debug.Log("Card gender is Undefined");
                break;
            case Struct.Gender.Woman:
                womanScore += upDelta*womanMultiplier;
                
                manScore -= downDelta;
                
                grannyScore -= downDelta;
                
                break;
            case Struct.Gender.Man:
                manScore += upDelta*manMultiplier;
                womanScore -= downDelta;
                grannyScore -= downDelta;
                break;
            case Struct.Gender.Granny:
                grannyScore += upDelta*grannyMultiplier;
                manScore -= downDelta;
                womanScore -= downDelta;
                break;
        }

        PreventNegative();
        UpdateStatusBar();
    }

    public void AddCoin()
    {
        _coins++;

        coinsText.text = _coins.ToString();
    }

    public void ProtectStore()
    {
        if(_coins >= priceProtect)
        {
            _currentStepsProtect += 5;
            _coins -= priceProtect;

            coinsText.text = _coins.ToString();
        }

        stepsProtectText.text = _currentStepsProtect.ToString();
    }

    public void DoStep()
    {
        if(_currentStepsProtect > 0)
        {
            _currentStepsProtect--;
        }

        stepsProtectText.text = _currentStepsProtect.ToString();
    }

    public void MixCards()
    {
        FindObjectOfType<SpawnGrid>().MixCards();
    }
}
