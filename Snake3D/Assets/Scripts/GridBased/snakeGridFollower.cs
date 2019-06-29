using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeGridFollower : MonoBehaviour
{

    private Vector3 direction;
    public Vector3 currentBlock;
    private Vector3 nextBlock;
    private List<PosDirection> blocks;
    public snakeGridFollower tail;
    public float speed;

    void Start()
    {
        blocks = new List<PosDirection>();
        direction = Vector3.forward;
        nextBlock = this.transform.position;
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
                PushToTail();
                GetNextBLock();
                this.transform.position = Vector3.MoveTowards(this.transform.position, nextBlock, speed * Time.deltaTime);
            }
        }
    }

    private void PushToTail()
    {
        if (tail != null)
        {
            tail.PushNextBlock(new PosDirection() { block = nextBlock, dir = direction });
        }
    }

    void GetNextBLock()
    {
        currentBlock = nextBlock;
        if (blocks!=null && blocks.Count > 0)
        {
            nextBlock = blocks[0].block;
            direction = blocks[0].dir;
            blocks.RemoveAt(0);
        }
        else
        {
            nextBlock = this.transform.position;
        }

    }

    public void PushNextBlock(PosDirection pos)
    {
        blocks.Add(pos);
    }


}

public struct PosDirection{

    public Vector3 block;
    public Vector3 dir;
}
