using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;

    [SerializeField] private Button winExitButton;
    [SerializeField] private Button winMainMenuButton;

    [SerializeField] private Button loseExitButton;
    [SerializeField] private Button loseMainMenuButton;

    private void Start()
    {
        winExitButton.onClick.AddListener(Exit);
        winMainMenuButton.onClick.AddListener(MainMenu);

        loseExitButton.onClick.AddListener(Exit);
        loseMainMenuButton.onClick.AddListener(MainMenu);

        //Hide panels on start
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0f; // Pause the game
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void WinGame()
    {
        Time.timeScale = 0f; // Pause the game
        winPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // Load Main Menu (assuming it's scene index 0)
    }

    private void Exit()
    {
        Application.Quit();
    }
}
