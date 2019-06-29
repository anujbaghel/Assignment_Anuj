using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

public class Manager : MonoBehaviour
{
    private string fileName = "config.json";
    public FoodCollection food;
    public GameObject foodPrefab;
    public snakeMovement head;
    private FoodEntity previousFood = null;
    private int streakOf;
    public  UIManager uiManager;
    private int score;
    public static bool isGameOver;
    private PrefsManager prefs;

    [Header("Bounds")]
    public Rect bounds;

    void OnEnable()
    {
        head.onFoodCollected += OnFoodCollected;
        head.gameOver += OnGameOver;
    }

    private void OnDisable()
    {
        head.onFoodCollected -= OnFoodCollected;
        head.gameOver -= OnGameOver;

    }

    async void Start()
    {
        food =  await LoadDataFromConfig();
        prefs = new PrefsManager();

        SpawnFood();

    }


    public void SpawnFood()
    {
        if (food != null && food.foods != null)
        {
            int randomIndex = UnityEngine.Random.Range(0, food.foods.Count);
            float x = UnityEngine.Random.Range(-bounds.xMax, bounds.xMax);
            float z = UnityEngine.Random.Range(-bounds.yMax, bounds.yMax);

            GameObject foodObjcet = Instantiate(foodPrefab);
            foodObjcet.transform.position = new Vector3(x, 0.5f, z);
            FoodProperties properties = foodObjcet.GetComponent<FoodProperties>();
            properties.SetFoodType(food.foods[randomIndex]);

        }
    }

    async Task<FoodCollection> LoadDataFromConfig()
    {
        FoodCollection food = null;
        string path = Path.Combine(Application.dataPath, fileName);

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);

            food = JsonUtility.FromJson<FoodCollection>(jsonData);
        }

        return food;
    }

    void OnFoodCollected(FoodEntity foodEntity)
    {
        SpawnFood();

        if (previousFood != null && previousFood == foodEntity)
        {
            streakOf++;
        }
        else
        {
            streakOf = 1;
        }

        score += streakOf * foodEntity.points;

        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
        }
        previousFood = foodEntity;
    }

    void OnGameOver()
    {
        isGameOver = true;
        uiManager.ShowGameOverPanel(score);
        prefs.SaveHighScore(score);
    }
}

[Serializable]
public class FoodCollection
{
    public List<FoodEntity> foods;
}
