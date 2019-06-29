using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridGameManager : MonoBehaviour
{
    public UIManager uiManager;
    public SnakeGridMovement snakeHead;
    private FoodEntity previousFood = null;
    private int streakOf;
    private Text scoreText;
    private int score;
    private PrefsManager prefs;
    public static bool isGameOver = false;


    private void OnEnable()
    {
        snakeHead.pickups += OnFoodCollected;
        snakeHead.gameOver += OnGameOver;
    }


    private void OnDisable()
    {
        snakeHead.pickups -= OnFoodCollected;
        snakeHead.gameOver -= OnGameOver;
    }

    private void Start()
    {
        isGameOver = false;
        prefs = new PrefsManager();
    }

    void OnFoodCollected(FoodEntity food)
    {
        if(previousFood!=null && previousFood == food)
        {
            streakOf++;
        }
        else
        {
            streakOf = 1;
        }

        score += streakOf * food.points;

        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
        }
        previousFood = food;
    }

    void OnGameOver()
    {
        isGameOver = true;
        uiManager.ShowGameOverPanel(score);
        prefs.SaveHighScore(score);
    }

}
