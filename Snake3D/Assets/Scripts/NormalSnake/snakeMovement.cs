using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class snakeMovement : MonoBehaviour
{
    public float speed;
    public GameObject followerPrefab;
    public snakeFollower snakeTail;
    private Rigidbody rb;
    private Vector3 direction;
    private Vector3 previousDir;
    private bool changeDir;
    public bool start;
    public snakeFollower Tail;
    public InputManager inputManager;

    public delegate void OnFoodCollected(FoodEntity entity);
    public delegate void OnGameOver();

    public event OnFoodCollected onFoodCollected;
    public event OnGameOver gameOver;

    private void OnEnable()
    {
        inputManager.OnInput += ChangeDirection;
    }

    private void OnDisable()
    {
        inputManager.OnInput -= ChangeDirection;

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = Vector3.forward;
        previousDir = direction;
    }

    void FixedUpdate()
    {
        if(!Manager.isGameOver)
        {
            rb.velocity = direction * speed;
            if (previousDir != direction)
            {
                changeDir = true;
            }
            previousDir = direction;
        }
        else
        {
            rb.velocity = direction * 0;
        }

    }   

    public void ChangeDirection(Vector3 dir)
    {
        previousDir = direction;
        direction = dir;
    }

    private void LateUpdate()
    {


        //if (start)
        //{
        //    snakeTail.UpdatPos(this.transform.position);
        //}

        if (changeDir)
        {
            if (snakeTail != null)
            {
                snakeTail.addPath(new pointDirection() { point = this.transform.position, direction = direction });
            }
            changeDir = false;
        }    
    }

 

    void CheckDistance()
    {
        if (snakeTail != null)
        {
            if (Vector3.Distance(this.transform.position, snakeTail.transform.position) >= 1.1f)
            {
                start = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Food":
                Destroy(other.gameObject);
                if (onFoodCollected != null)
                {
                    onFoodCollected(other.GetComponent<FoodProperties>().foodEntity);
                }
                AddSnake();
                break;
            case "follower":
                if (other.gameObject != snakeTail.gameObject)
                {
                    GameOver();
                }
                break;
            case "wall":
                GameOver();
                break;
        }
    }

    void GameOver()
    {
        if (gameOver != null)
        {
            gameOver();
        }

    }

    void AddSnake()
    {
        if(Tail== null)
        {
            GameObject newTail = Instantiate(followerPrefab, this.transform.position, Quaternion.identity);
            this.snakeTail = newTail.GetComponent<snakeFollower>();
            snakeTail.isTail = true;
            snakeTail.head = this;
            snakeTail.speed = this.speed;
            snakeTail.parent = this.transform;
            snakeTail.followerPrefab = this.followerPrefab;
            snakeTail.direction = this.direction;
        }
        else
            Tail.AddCHild();
    }
}



