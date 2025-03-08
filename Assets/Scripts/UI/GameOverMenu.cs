using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Button restartButton,exitButton;

    //Start is called before the first frame update
    void Start()
    {
        restartButton.onClick.AddListener(Restart);
        exitButton.onClick.AddListener(Exit);
    }

    private void Restart()
    {
        SceneManager.LoadScene(1);
    }

    private void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
