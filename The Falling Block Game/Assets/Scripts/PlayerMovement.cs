using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public float moveDist;

    public int currX, currY, currZ;

    Rigidbody rb;
    BlockSpawner blockSpawner;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        blockSpawner = BlockSpawner.Instance;
        this.transform.position = new Vector3(moveDist / 2.0f, moveDist / 2.0f, moveDist / 2.0f);
        currX = 0;
        currY = 0;
        currZ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currY = blockSpawner.grid[currX, currZ];
        restrictedMove();
    }

    void restrictedMove()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            addZ();
        } 
        else if (Input.GetKeyDown(KeyCode.S))
        {
            subZ();
        } 
        else if (Input.GetKeyDown(KeyCode.D))
        {
            addX();
        } 
        else if (Input.GetKeyDown(KeyCode.A))
        {
            subX();
        }
    }

    void addZ()
    {
        if (currZ + 1 < blockSpawner.gridZ)
        {
            int targetHeight = blockSpawner.grid[currX, currZ + 1];
            if (targetHeight <= currY + 1)
            {
                currZ++;
                transform.position = blockSpawner.gridToCoords(currX, targetHeight, currZ);
            }
        }
    }

    void subZ()
    {
        if (currZ - 1 >= 0)
        {
            int targetHeight = blockSpawner.grid[currX, currZ - 1];
            if (targetHeight <= currY + 1)
            {
                currZ--;
                transform.position = blockSpawner.gridToCoords(currX, targetHeight, currZ);
            }
        }
    }

    void addX()
    {
        if (currX + 1 < blockSpawner.gridX)
        {
            int targetHeight = blockSpawner.grid[currX + 1, currZ];
            if (targetHeight <= currY + 1)
            {
                currX++;
                transform.position = blockSpawner.gridToCoords(currX, targetHeight, currZ);
            }
        }
    }

    void subX()
    {
        if (currX - 1 >= 0)
        {
            int targetHeight = blockSpawner.grid[currX - 1, currZ];
            if (targetHeight <= currY + 1)
            {
                currX--;
                transform.position = blockSpawner.gridToCoords(currX, targetHeight, currZ);
            }
        }
    }

}
