using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Text depictionText;
        
    [SerializeField] private GameObject creatorsPanel;
    [SerializeField] private GameObject goToStartMenuButton;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject aboutPanel;
    [SerializeField] private GameObject startGamePanel1;
    [SerializeField] private GameObject startGamePanel2;
    [SerializeField] private GameObject startGamePanel3;

    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private GameObject backgroundSoundObject;
    [SerializeField] private GameObject creduitsSoundObject;
    private AudioSource audio;

    private bool _isCredits;

    [SerializeField] private float speedPrintText = 0.1f;

    private int indexText;

    [SerializeField] private Text[] creatorsTexts;
    private string[] creatorNamesTexts;
    void Awake()
    {
        audio = GetComponent<AudioSource>();

        creatorNamesTexts = new string[creatorsTexts.Length];

        for (var i = 0; i < creatorsTexts.Length; i++)
        {
            creatorNamesTexts[i] = creatorsTexts[i].text;
            creatorsTexts[i].text = string.Empty;
        }

        ShowStartMenu();
    }

    public void PlayClickButton()
    {
        audio.PlayOneShot(buttonSound);
    }

    public void OnStartGameButton()
    {
        menuPanel.SetActive(false);
        startGamePanel1.SetActive(true);
    }

    public void OnContinueButton1()
    {
        startGamePanel1.SetActive(false);
        startGamePanel2.SetActive(true);
    }

    public void OnContinueButton2()
    {
        startGamePanel2.SetActive(false);
        startGamePanel3.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }


    public void OnAboutButton()
    {
        menuPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }
    public void OnCreatorsButton()
    {
        backgroundSoundObject.GetComponent<AudioSource>().Stop();
        creduitsSoundObject.GetComponent<AudioSource>().Play();

        menuPanel.SetActive(false);
        creatorsPanel.SetActive(true);
        goToStartMenuButton.SetActive(false);

        StartCoroutine(SpellingCreators());

        _isCredits = true;
    }

    IEnumerator Spelling(Text outputText, string inputText)
    {
        for (var i = 0; i <= inputText.Length; i++)
        {
            outputText.text = inputText.Substring(0, i);

            yield return new WaitForSeconds(speedPrintText);
        }
    }

    IEnumerator SpellingCreators()
    {
        for (var i = 0; i < creatorNamesTexts.Length; i++)
        {
            var text = creatorNamesTexts[i];
            yield return Spelling(creatorsTexts[i], text);
        }

        goToStartMenuButton.SetActive(true);
    }
    private void ShowStartMenu()
    {
        menuPanel.SetActive(true);
        creatorsPanel.SetActive(false);
        aboutPanel.SetActive(false);
    }

    public void OnBackToStartMenu()
    {
        if (_isCredits)
        {
            backgroundSoundObject.GetComponent<AudioSource>().Play();
            creduitsSoundObject.GetComponent<AudioSource>().Stop();
        }
        _isCredits = false;

        ShowStartMenu();
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
