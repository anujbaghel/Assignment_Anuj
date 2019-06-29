using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Text scoreText, gameOverText;
    public GameObject GameOverPanel;

    private void Start()
    {
        GameOverPanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score : " + score;
    }

    public void ShowGameOverPanel(int score)
    {
        Debug.Log("Game Over");
        if (GameOverPanel != null)
        {
            GameOverPanel.SetActive(true);
            gameOverText.text = score + "";
        }
    }

    public void OnHomePressed()
    {
        Debug.Log("Home");
        SceneManager.LoadScene(0);
    }
        
}
