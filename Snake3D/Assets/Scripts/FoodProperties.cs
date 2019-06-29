using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodProperties : MonoBehaviour
{
    public FoodEntity foodEntity;

    void Start()
    {
        
    }

    void SetColor()
    {
        this.GetComponent<Renderer>().material.color = GetColor(foodEntity.foodColor);
    }

    public void SetFoodType(FoodEntity food)
    {
        foodEntity = food;
        SetColor();
    }

    Color GetColor(string name)
    {
        name = name.ToUpper();

        switch (name)
        {
            case "RED":
                return Color.red;
            case "BLUE":
                return Color.blue;
            default:
                return Color.black;
        }
    }

   
}
