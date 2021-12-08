using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public float moveDist;

    public int currX, currZ;

    Rigidbody rb;
    BlockSpawner blockSpawner;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        blockSpawner = BlockSpawner.Instance;
        Vector3 currPos = this.transform.position;
        currPos -= new Vector3(-moveDist / 2, 0, -moveDist / 2);
        currPos *= (1.0f / moveDist);
        int currX = Mathf.RoundToInt(currPos.x);
        int currZ = Mathf.RoundToInt(currPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        //RawMovement();
        //RawJump();
        restrictedMove();
    }

    void restrictedMove()
    {
        int currY = blockSpawner.grid[currX, currZ];
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currZ + 1 < blockSpawner.gridZ)
            {
                int targetHeight = blockSpawner.grid[currX, currZ + 1];
                if (targetHeight <= currY + 1)
                {
                    Vector3 relativeMove = new Vector3(0, (targetHeight - currY), 1) * moveDist;
                    currZ++;
                    transform.position = blockSpawner.gridToCoords(currX, targetHeight, currZ);
                }
            }
        } 
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if(currZ - 1 >= 0)
            {
                int targetHeight = blockSpawner.grid[currX, currZ - 1];
                if (targetHeight <= currY + 1)
                {
                    Vector3 relativeMove = new Vector3(0, (targetHeight - currY), -1) * moveDist;
                    currZ--;
                    transform.position = blockSpawner.gridToCoords(currX, targetHeight, currZ);
                }
            }
        } 
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (currX + 1 < blockSpawner.gridX)
            {
                int targetHeight = blockSpawner.grid[currX + 1, currZ];
                if (targetHeight <= currY + 1)
                {
                    Vector3 relativeMove = new Vector3(1, (targetHeight - currY), 0) * moveDist;
                    currX++;
                    transform.position = blockSpawner.gridToCoords(currX, targetHeight, currZ);
                }
            }
        } 
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (currX - 1 >= 0)
            {
                int targetHeight = blockSpawner.grid[currX - 1, currZ];
                if (targetHeight <= currY + 1)
                {
                    Vector3 relativeMove = new Vector3(-1, (targetHeight - currY), 0) * moveDist;
                    currX--;
                    transform.position = blockSpawner.gridToCoords(currX, targetHeight, currZ);
                }
            }
        }
    }


}
