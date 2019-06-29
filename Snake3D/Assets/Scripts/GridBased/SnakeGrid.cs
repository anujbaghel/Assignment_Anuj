using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class SnakeGrid : MonoBehaviour
{

    private string fileName = "config.json";
    public FoodCollection foodCollection;

    [Header("Grid")]
    public int columns;
    public int rows;


    [Header("GameObjects")]
    public SnakeGridMovement snakeHead;
    public GameObject block, food, boundHolder;

    public int[,] grid;

    private void OnEnable()
    {
        snakeHead.pickups += OnFoodCollected;
    }

    private void OnDisable()
    {
        snakeHead.pickups -= OnFoodCollected;
    }

    async void Start()
    {
        CreateGrid();
        SetWalls();

        foodCollection = await LoadDataFromConfig();
        SpawnFood();
    }

    void CreateGrid()
    {
        grid = new int[rows, columns];
        for(int column = 0; column < columns; column++)
        {
            for(int row = 0; row < rows; row++)
            {
                Instantiate(block, new Vector3(row, 0,column), Quaternion.Euler(90,0,0),this.transform);
            }
        }
    }

    void SetWalls()
    {
        if(columns == rows)
        {
            float xLenght = columns;
            float zLength = xLenght + 2f;
            float pos = ((float)columns / 2f) - 0.5f;
            boundHolder.transform.position = new Vector3(pos, 0, pos);
            Vector3 holderPos = boundHolder.transform.position;
            Camera.main.transform.position = new Vector3(pos, Camera.main.transform.position.y, pos);

            if (columns > 10)
            {
                Camera.main.fieldOfView += (columns - 6);
            }

            pos += 1;
            for(int i = 0; i < 4; i++)
            {
                Transform wall = boundHolder.transform.GetChild(i);
                Vector3 localS = wall.localScale;
                Vector3 positionS = wall.position;
                switch (i)
                {
                    case 0: //Left Child
                        localS.z = zLength;
                        positionS.x = -pos;
                        positionS.z = 0;
                        break;
                    case 1: //Right Child
                        localS.z = zLength;
                        positionS.x = pos;
                        positionS.z = 0;
                        break;
                    case 2:  //Top CHild
                        localS.x = xLenght;
                        positionS.z = pos;
                        positionS.x = 0;
                        break;
                    case 3: //Bottom CHild
                        localS.x = xLenght;
                        positionS.z = -pos;
                        positionS.x = 0;
                        break;
                }


                wall.localScale = localS;
                positionS = new Vector3(holderPos.x + positionS.x, holderPos.y + positionS.y, holderPos.z + positionS.z);
                wall.position = positionS;
            }
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

    public void SpawnFood()
    {

        if (foodCollection != null && foodCollection.foods != null)
        {
            int randomIndex = UnityEngine.Random.Range(0, foodCollection.foods.Count);

            GameObject f = Instantiate(food);

            //Placing food in the grid
            Vector2 randomXY = GetRandomXY();   

            f.transform.position = new Vector3(randomXY.x, 0, randomXY.y);

            FoodProperties properties = f.GetComponent<FoodProperties>();
            properties.SetFoodType(foodCollection.foods[randomIndex]);

        }

      
    }

    Vector2 GetRandomXY()
    {
        Vector2 randomXY = new Vector2();
        randomXY.x = Random.Range(0, rows);
        randomXY.y = Random.Range(0, columns);

        while((randomXY.x == snakeHead.nextBlock.x && randomXY.y == snakeHead.nextBlock.y) || ( randomXY.x == snakeHead.previousBlock.x && randomXY.y == snakeHead.previousBlock.y)){
            randomXY.x = Random.Range(0, rows);
            randomXY.y = Random.Range(0, columns);
        }
        return randomXY;
    }

    public void OnFoodCollected(FoodEntity food)
    {
        SpawnFood();
    }

}
