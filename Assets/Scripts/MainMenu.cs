using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton, settingsButton, creditsButton, closeCreditsButton, exitButton;
    [SerializeField] private GameObject settingsMenu, creditsMenu; //Reference to UI Panels

    //Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(Play);
        exitButton.onClick.AddListener(Exit);
        settingsButton.onClick.AddListener(OpenSettings);
        creditsButton.onClick.AddListener(OpenCredits);
        closeCreditsButton.onClick.AddListener(CloseCredits);
    }

    //Update is called once per frame
    void Update()
    {
        
    }

    private void Play()
    {
        SceneManager.LoadScene(1);
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void OpenSettings()
    {
        settingsMenu.SetActive(true);
    }

    private void OpenCredits()
    {
        creditsMenu.SetActive(true);
    }

    private void CloseCredits()
    {
        creditsMenu.SetActive(false);
    }
}
