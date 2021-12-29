using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    private void Awake()
    {
        Instance = this;
    }

    public int currX, currY, currZ;
    float moveDist;

    Rigidbody rb;
    BlockSpawner blockSpawner;
    FixedThirdPersonCam gameCam;
    private int amountMapDropped = 0;
    public int maxPlayerHeight = 5;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        blockSpawner = BlockSpawner.Instance;
        gameCam = FixedThirdPersonCam.Instance;
        moveDist = GameSettings.blockSize;
        rb.MovePosition(new Vector3(moveDist / 2.0f, moveDist / 4.0f, moveDist / 2.0f));
        currX = 0;
        currY = 0;
        currZ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currY = blockSpawner.grid[currX, currZ];
        restrictedMove();
        mapDrop();
        checkHeightIncrease();
    }

    void checkHeightIncrease()
    {
        if (currY > maxPlayerHeight)
        {
            Debug.Log("max height: " + maxPlayerHeight);
            blockSpawner.incrementDespawnHeight();
            maxPlayerHeight = currY;
        }
        if (currY > GameSettings.playerScore)
        {
            GameSettings.playerScore = currY;
        }
    }

    public void mapDrop()
    {
        if (amountMapDropped < blockSpawner.amountMapDropped)
        {
            if (transform.position.y > 1.01)
            {
                Vector3 newPos = transform.position - Vector3.up * GameSettings.blockSize;
                rb.MovePosition(newPos);
            }
            amountMapDropped++;
        }
    }
    void restrictedMove()
    {
        switch(gameCam.currPos) 
        {
            case 0:
                restrictedMove0();
                break;

            case 1:
                restrictedMove1();
                break;

            case 2:
                restrictedMove2();
                break;

            case 3:
                restrictedMove3();
                break;
        }
        GameSettings.playerX = currX;
        GameSettings.playerY = currY;
        GameSettings.playerZ = currZ;
    }
    void restrictedMove0()
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
    void restrictedMove1()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            addZ();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            subZ();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            addX();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            subX();
        }
    }
    void restrictedMove2()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            addZ();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            subZ();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            addX();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            subX();
        }
    }
    void restrictedMove3()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            addZ();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            subZ();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            addX();
        }
        else if (Input.GetKeyDown(KeyCode.S))
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
                rb.MovePosition(blockSpawner.gridToCoords(currX, targetHeight, currZ));
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
                rb.MovePosition(blockSpawner.gridToCoords(currX, targetHeight, currZ));
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
                rb.MovePosition(blockSpawner.gridToCoords(currX, targetHeight, currZ));
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
                rb.MovePosition(blockSpawner.gridToCoords(currX, targetHeight, currZ));
            }
        }
    }

}
