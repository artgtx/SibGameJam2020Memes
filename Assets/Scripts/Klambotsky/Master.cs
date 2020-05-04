using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Master : MonoBehaviour
{
    public StatusBarController grannyStatusBar;
    public StatusBarController womanStatusBar;
    public StatusBarController manStatusBar;

    [SerializeField] private Text coinsText;

    [SerializeField] private GameObject grannyBuff;
    [SerializeField] private GameObject womanBuff;
    [SerializeField] private GameObject manBuff;

    [SerializeField] private Sprite debuffSprite;
    [SerializeField] private Sprite boostSprite;

    [SerializeField] private GameObject shieldObject;

    [SerializeField] private Text progressGrannyText;
    [SerializeField] private Text progressWomanText;
    [SerializeField] private Text progressManText;

    [SerializeField] private GameObject endGameObject;
    [SerializeField] private Image endGameImage;
    [SerializeField] private Sprite failGameGrannySprite;
    [SerializeField] private Sprite failGameWomanSprite;
    [SerializeField] private Sprite failGameManSprite;
    [SerializeField] private Sprite winGameSprite;

    [SerializeField] private Button endGameButton;
    [SerializeField] private Sprite restartButtonSprite;
    [SerializeField] private Sprite continueButtonSprite;

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
    public float defaultMultiplier = 1;

    // PlayerScore
    private int startScore = 200;
    public float womanScore;
    public float manScore;
    public float grannyScore;

    public int _coins = 0;

    private int _currentStepsProtect;
    public int stepsProtect = 5;
    public int protect = 5;
    public int priceProtect = 2;

    private int _currentStepsBoostGranny;
    private int _currentStepsBoostWoman;
    private int _currentStepsBoostMan;
    public int stepsBoost = 5;
    public int boost = 2;
    public int priceBoost = 0;

    private int _currentStepsDebuffGranny;
    private int _currentStepsDebuffWoman;
    private int _currentStepsDebuffMan;
    public int stepsDebuff = 5;
    public float debuff = 0.5f;
    public int priceDebuff = 0;

    private int _countStepsFrozenGranny;
    private int _countStepsFrozenWoman;
    private int _countStepsFrozenMan;
    public int _countStepsFrozen = 10;

    private bool _isWin;
    private void Start()
    {
        Singletone.canGo = true;
        Singletone.canMove = true;

        womanScore = startScore;
        manScore = startScore;
        grannyScore = startScore;
        UpdateStatusBar();
        UpdateScoreTextes();

        endGameObject.SetActive(false);
    }

    private void UpdateScoreTextes()
    { 
        progressGrannyText.text = grannyScore.ToString();
        progressWomanText.text = womanScore.ToString();
        progressManText.text = manScore.ToString();

        ChangeColorText(progressGrannyText, grannyScore / maxCount * 100);
        ChangeColorText(progressWomanText, womanScore / maxCount * 100);
        ChangeColorText(progressManText, manScore / maxCount * 100);
    }

    private void ChangeColorText(Text text, float persent)
    {
        if (persent <= 20)
        {
            text.color = Color.red;
            return;
        }
        if (persent > 20 && persent < 70)
        {
            text.color = Color.green;
            return;
        }
        if (persent >= 70)
        {
            text.color = Color.yellow;
            return;
        }
    }

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
                {
                    ChangeScore(ref _countStepsFrozenWoman, ref womanScore, -value);
                    break;
                }
            case Struct.Gender.Man:
                {
                    ChangeScore(ref _countStepsFrozenMan, ref manScore, -value);
                    break;
                }
            case Struct.Gender.Granny:
                {
                    ChangeScore(ref _countStepsFrozenGranny, ref grannyScore, -value);
                    break;
                }
        }
        PreventNegative();
        UpdateStatusBar();
        UpdateScoreTextes();
        CheckEndGame();
    }

    private void ChangeScore(ref int countSteps, ref float score, float value)
    {
        if (countSteps <= 0)
        {
            score += value;
            if(score > maxCount)
            {
                score = maxCount;
            }
        }
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
                ChangeScore(ref _countStepsFrozenWoman, ref womanScore, upDelta * womanMultiplier);
                ChangeScore(ref _countStepsFrozenMan, ref manScore, -downDelta);
                ChangeScore(ref _countStepsFrozenGranny, ref grannyScore, -downDelta);
                break;
            case Struct.Gender.Man:
                ChangeScore(ref _countStepsFrozenWoman, ref womanScore, -downDelta);
                ChangeScore(ref _countStepsFrozenMan, ref manScore, upDelta * manMultiplier);
                ChangeScore(ref _countStepsFrozenGranny, ref grannyScore, -downDelta);
                break;
            case Struct.Gender.Granny:
                ChangeScore(ref _countStepsFrozenWoman, ref womanScore, -downDelta);
                ChangeScore(ref _countStepsFrozenMan, ref manScore, -downDelta);
                ChangeScore(ref _countStepsFrozenGranny, ref grannyScore, upDelta * grannyMultiplier);
                break;
        }

        PreventNegative();
        UpdateStatusBar();
        UpdateScoreTextes();
        CheckEndGame();
        CheckWinGame();
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
            _currentStepsProtect = 5;
            _coins -= priceProtect;

            coinsText.text = _coins.ToString();
        }

        shieldObject.GetComponent<ProtectController>().AddValue(_currentStepsProtect);
    }

    public void DoStep()
    {
        if(_currentStepsProtect > 0)
        {
            _currentStepsProtect--;
        }

        CheckBuff(womanBuff, ref _currentStepsBoostWoman, ref _currentStepsDebuffWoman, ref womanMultiplier);
        CheckBuff(manBuff, ref _currentStepsBoostMan, ref _currentStepsDebuffMan, ref manMultiplier);
        CheckBuff(grannyBuff, ref _currentStepsBoostGranny, ref _currentStepsDebuffGranny, ref grannyMultiplier);

        shieldObject.GetComponent<ProtectController>().SubValue(1);

        CheckFrozen();
    }

    private void CheckFrozen()
    {
        if(_countStepsFrozenGranny > 0)
        {
            _countStepsFrozenGranny--;
        }
        if (_countStepsFrozenMan > 0)
        {
            _countStepsFrozenMan--;
        }
        if (_countStepsFrozenWoman > 0)
        {
            _countStepsFrozenWoman--;
        }
    }
    private void CheckBuff(GameObject buffObject, ref int stepsBoost, ref int stepsDebuff, ref float multiplier)
    {
        if (stepsBoost > 0)
        {
            stepsBoost--;
            return;
        }
        if(stepsDebuff > 0)
        {
            stepsDebuff--;
            return;
        }

        multiplier = defaultMultiplier;
        buffObject.GetComponent<SpriteRenderer>().sprite = null;
    }

    public void BoostScore(Struct.Gender gender)
    {
        BuffScore(gender, true);
    }

    public void BuffScore(Struct.Gender gender, bool isBoost)
    {
        var price = isBoost ? priceBoost : priceDebuff;
        var steps = isBoost ? stepsBoost : stepsDebuff;
        var multiplier = isBoost ? boost : debuff;
        var sprite = isBoost ? boostSprite : debuffSprite;

        if (_coins >= price)
        {
            _coins -= price;

            switch (gender)
            {
                case Struct.Gender.Granny:
                    {
                        if (isBoost)
                        {
                            _currentStepsBoostGranny += steps;
                        }
                        else
                        {
                            _currentStepsDebuffGranny += steps;
                        }
                        grannyMultiplier = multiplier;
                        grannyBuff.GetComponent<SpriteRenderer>().sprite = sprite;
                        break;
                    }
                case Struct.Gender.Woman:
                    {
                        if (isBoost)
                        {
                            _currentStepsBoostWoman += steps;
                        }
                        else
                        {
                            _currentStepsDebuffWoman += steps;
                        }
                        womanMultiplier = multiplier;
                        womanBuff.GetComponent<SpriteRenderer>().sprite = sprite;
                        break;
                    }
                case Struct.Gender.Man:
                    {

                        if (isBoost)
                        {
                            _currentStepsBoostMan += steps;
                        }
                        else
                        {
                            _currentStepsDebuffMan += steps;
                        }
                        manMultiplier = multiplier;
                        manBuff.GetComponent<SpriteRenderer>().sprite = sprite;
                        break;
                    }
            }
        }
    }

    public void DebaffScore(Struct.Gender gender)
    {
        BuffScore(gender, false);
    }

    private void CheckEndGame()
    {
        if(womanScore <= 0)
        {
            EndGame(failGameWomanSprite, restartButtonSprite, false);
            return;
        }
        if (manScore <= 0)
        {
            EndGame(failGameManSprite, restartButtonSprite, false);
            return;
        }
        if (grannyScore <= 0)
        {
            EndGame(failGameGrannySprite, restartButtonSprite, false);
            return;
        }
    }

    private void CheckWinGame()
    {
        if (womanScore >= maxCount && manScore >= maxCount && grannyScore >= maxCount)
        {
            EndGame(winGameSprite, continueButtonSprite, true);
            return;
        }

        if (womanScore >= maxCount)
        {
            womanScore = maxCount;
            if(_countStepsFrozenWoman <= 0)
            {
                _countStepsFrozenWoman = _countStepsFrozen;
            }
        }
        if (manScore >= maxCount)
        {
            manScore = maxCount;
            if (_countStepsFrozenMan <= 0)
            {
                _countStepsFrozenMan = _countStepsFrozen;
            }
        }
        if (grannyScore >= maxCount)
        {
            grannyScore = maxCount;
            if (_countStepsFrozenGranny <= 0)
            {
                _countStepsFrozenGranny = _countStepsFrozen;
            }
        }
    }

    private void EndGame(Sprite spriteImage, Sprite spriteButton, bool isWin)
    {
        
        _isWin = isWin;

        Singletone.canGo = false;
        endGameImage.sprite = spriteImage;
        endGameButton.image.sprite = spriteButton;
        endGameObject.SetActive(true);
        if (isWin)
        {
            gameSound.instance.onWinSound();
        }
        else
        {
            gameSound.instance.onLoseSound();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Singletone.canGo = true;
        Singletone.canMove = true;
    }

    public void ContinueGameButton()
    {
        gameSound.instance.onBtnPsrSound();
        if (_isWin)
        {
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            RestartGame();
        }
    }
}
