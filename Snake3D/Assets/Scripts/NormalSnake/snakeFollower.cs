using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeFollower : MonoBehaviour
{
    public float speed;
    Rigidbody rb;
    public Transform parent;

    public snakeMovement head;

    public snakeFollower  snakeFollow;
    public List<pointDirection> path;
    public Vector3 direction;
    public pointDirection pointDir;

    public bool isTail;
    public GameObject followerPrefab;
    public bool startFollow;
    float tempSpeed;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        path = new List<pointDirection>();

        if(snakeFollow == null)
        {
            isTail = true;
            head.Tail = this;
        }
        tempSpeed = speed;
    }

    private void Update()
    {
        if (!Manager.isGameOver)
        {
            if (path != null && path.Count > 0)
            {
                pointDir = path[0];

                if (ReachedCheckPoint(pointDir))
                {
                    Debug.Log("Point is " + pointDir.point);
                    direction = pointDir.direction;

                    if (snakeFollow != null)
                    {
                        snakeFollow.addPath(path[0]);
                    }

                    path.RemoveAt(0);
                }
            }

            CheckDistance();
        }
       

        //if (path.Count > 0 && CheckDistance())
        //{
        //    if(Math.Round(path[0].z,2) == Math.Round(this.transform.position.z,2) && Math.Round(path[0].x,2) == Math.Round(this.transform.position.x,2))
        //    {
        //        path.RemoveAt(0);
        //        iTween.MoveUpdate(gameObject, path[0], 1f);

        //    }
        //    else
        //    {
        //        iTween.MoveUpdate(gameObject, path[0], 1);
        //    }
        //}


    }

    void FixedUpdate()
    {
        if (startFollow && !Manager.isGameOver)
        {
            rb.velocity = direction * speed;
        }
        else
        {
            rb.velocity = direction * 0;
        }

    }


    //public void UpdatPos(Vector3 pos)
    //{
    //    path.Add(pos);
    //   // iTween.MoveUpdate(gameObject, pos, 1f);
    //}

    void CheckDistance()
    {
        float distance = Vector3.Distance(this.transform.position, parent.position);
        if (distance >= 0.9f)
        {
            speed = tempSpeed + 1.5f;
            startFollow = true;
        }
        else
        {
            speed = tempSpeed;
        }
    }

    public void addPath(pointDirection point)
    {
        path.Add(point);

     
    }

    bool ReachedCheckPoint(pointDirection pointDirection)
    {
        if (direction == Vector3.forward && pointDirection.point.z + 0.05f <= this.transform.position.z)
        {
            return true;
        }
        else if (direction == Vector3.left && pointDirection.point.x - 0.05f >= this.transform.position.x)
        {
            return true;
        }
        else if (direction == Vector3.back && pointDirection.point.z - 0.05f >= this.transform.position.z)
        {
            return true;
        }
        else if (direction == Vector3.right && pointDirection.point.x + 0.05f <= this.transform.position.x)
        {
            return true;
        }

        return false;
    }

    public void AddCHild()
    {
        GameObject newTail = Instantiate(followerPrefab, this.transform.position, Quaternion.identity);
        this.snakeFollow = newTail.GetComponent<snakeFollower>();
        snakeFollow.isTail = true;
        snakeFollow.speed = this.speed;
        snakeFollow.head = head;
        snakeFollow.parent = this.transform;
        snakeFollow.followerPrefab = this.followerPrefab;
        snakeFollow.direction = this.direction;
        this.isTail = false;
    }

}

public struct pointDirection
{
    public Vector3 point;
    public Vector3 direction;
}

