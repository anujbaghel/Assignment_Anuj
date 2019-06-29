using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUiManager : MonoBehaviour
{
    public Text highScore;
    public GameObject choicePanel;

    private PrefsManager prefs;

    void Start()
    {
        choicePanel.SetActive(false);
        prefs = new PrefsManager();
        SetHighScore(prefs.GetHighScore());
    }


    void SetHighScore(int score)
    {
        highScore.text = "HIGH SCORE : " + score;
    }

    public void OnPlayClick()
    {
        choicePanel.SetActive(true);
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnGridClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnNormalClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

    }
}
