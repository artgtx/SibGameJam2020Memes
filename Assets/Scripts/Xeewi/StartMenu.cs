using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Text creatorsText;
    [SerializeField] private Text depictionText;

    [SerializeField] private GameObject creatorsPanel;
    [SerializeField] private GameObject goToStartMenuButton;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject aboutPanel;
    [SerializeField] private GameObject startGamePanel;

    [SerializeField] private AudioClip buttonSound;
    private AudioSource audio;

    [SerializeField] private float speedPrintText = 0.1f;

    private string[] texts = new string[3];

    private int indexText;

    private const string textOne = "Text 1";
    private const string textTwo = "Text 2";
    private const string textThree = "Text 3";

    private const string CreatorsString = "Программист\n" +
        "Программист\n" +
        "Программист\n" +
        "Художник\n" +
        "Художник\n" +
        "Художник\n"+
        "Интерфейс\n" +
        "Геймдизайн\n";
  
    void Awake()
    {
        texts[0] = textOne;
        texts[1] = textTwo;
        texts[2] = textThree;

        audio = GetComponent<AudioSource>();

        ShowStartMenu();
    }

    public void OnPlayButton()
    {
        menuPanel.SetActive(false);
        startGamePanel.SetActive(true);

        StartCoroutine(ShowCreators(depictionText, texts[indexText++]));
    }

    public void PlayClickButton()
    {
        audio.PlayOneShot(buttonSound);
    }

    public void OnStartGameButton()
    {
        if(indexText == texts.Length)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            StartCoroutine(ShowCreators(depictionText, texts[indexText++]));
        }
    }
    public void OnAboutButton()
    {
        menuPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }
    public void OnCreatorsButton()
    {
        menuPanel.SetActive(false);
        creatorsPanel.SetActive(true);
        goToStartMenuButton.SetActive(false);

        StartCoroutine(ShowCreators(creatorsText, CreatorsString));
    }

    IEnumerator ShowCreators(Text outputText, string inputText)
    {
        for (var i = 0; i < inputText.Length; i++)
        {
            outputText.text = inputText.Substring(0, i);

            yield return new WaitForSeconds(speedPrintText);
        }

        goToStartMenuButton.SetActive(true);
    }

    private void ShowStartMenu()
    {
        menuPanel.SetActive(true);
        creatorsPanel.SetActive(false);
        aboutPanel.SetActive(false);
        startGamePanel.SetActive(false);
    }

    public void OnBackToStartMenu()
    {
        ShowStartMenu();
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
