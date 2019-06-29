using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGridMovement : MonoBehaviour
{

    public float speed;
    public GameObject TailPrefab;
    private Vector3 direction;
    private Vector3 previousDir;
    public Vector3 nextBlock, previousBlock;
    public snakeGridFollower child,tail;

    public InputManager inputManager;

    public delegate void OnFoodPickup(FoodEntity food);
    public delegate void OnGameOver();

    public event OnGameOver gameOver;
    public event OnFoodPickup pickups;


    private void OnEnable()
    {
        inputManager.OnInput += ChangeDirection;
    }

    void Start()
    {
        direction = Vector3.forward;
        GetNextBLock();

    }

    void Update()
    {
        if (!GridGameManager.isGameOver)
        {
            if (this.transform.position != nextBlock)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, nextBlock, speed * Time.deltaTime);
            }
            else
            {
                if (child != null)
                {
                    child.PushNextBlock(new PosDirection() { block = nextBlock, dir = previousDir });
                }
                previousBlock = nextBlock;
                GetNextBLock();
                this.transform.position = Vector3.MoveTowards(this.transform.position, nextBlock, speed * Time.deltaTime);
            }
        }
    }

    public void ChangeDirection(Vector3 dir)
    {
        if(-direction != dir) {
            previousDir = direction;
            direction = dir;
        }
        else
        {
            Debug.Log("Backward movement not allowed");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Food":
                FoodCollision(other);
                break;
            case "follower":
                TailCollision(other);
                break;
            case "wall":
                GameOver();
                break;
        }

    }

    void GetNextBLock()
    {
        if(direction == Vector3.forward)
        {
            nextBlock.z += 1;
        }
        if (direction == Vector3.back)
        {
            nextBlock.z -= 1;
        }
        if (direction == Vector3.left)
        {
            nextBlock.x -= 1;
        }
        if (direction == Vector3.right)
        {
            nextBlock.x += 1;
        }
    }

    void TailCollision(Collider other)
    {
        if(other.gameObject != child.gameObject)
        {
            GameOver();
        }
    }


    void GameOver()
    {
        if (gameOver != null)
        {
            gameOver();
        }
    }


    void FoodCollision(Collider other)
    {
            SpawnTail();
            Destroy(other.gameObject);

            if (pickups != null)
            {
                pickups(other.GetComponent<FoodProperties>().foodEntity);
            }
    }

    public void SpawnTail()
    {
        GameObject tailObject = Instantiate(TailPrefab);
        snakeGridFollower tailFollower = tailObject.GetComponent<snakeGridFollower>();
        if (tail != null)
        {
            tailObject.transform.position = tail.currentBlock;
            tail.tail = tailFollower;
            this.tail = tail.tail;
        }
        else
        {
            tailObject.transform.position = previousBlock;
            this.tail = tailFollower;
            this.child = tailFollower;
        }
        tailFollower.speed = this.speed;

    }
}
