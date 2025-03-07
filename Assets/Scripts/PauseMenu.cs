using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button ResumeButton, MainMenuButton, settingsButton, exitButton;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    private bool isPaused = false;

    //Start is called before the first frame update
    void Start()
    {
        ResumeButton.onClick.AddListener(Resume);
        MainMenuButton.onClick.AddListener(MainMenu);
        exitButton.onClick.AddListener(Exit);
        settingsButton.onClick.AddListener(OpenSettings);
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (!isPaused)
                Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; //Freeze game
        pauseMenu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;// Unlock cursor
        Cursor.visible = true; //Show cursor
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; //Resume game
        pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked; //Lock cursor
        Cursor.visible = false; //Hide cursor
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true); //Show settings
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f; //Reset time before loading menu
        SceneManager.LoadScene(0); //Ensure Main Menu is at index 0
    }

    public void Exit()
    {
        Application.Quit();
    }
}
