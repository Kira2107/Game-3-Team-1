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
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            playButton.onClick.AddListener(Play);
            exitButton.onClick.AddListener(Exit);
            creditsButton.onClick.AddListener(OpenCredits);
            closeCreditsButton.onClick.AddListener(CloseCredits);
            settingsButton.onClick.AddListener(OpenSettings);
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            exitButton.onClick.AddListener(Exit);
        }
        
    }

    //Update is called once per frame
    void Update()
    {
        // Cursor.visible = true;
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
