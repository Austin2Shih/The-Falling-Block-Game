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
        currY = blockSpawner.heightMap[currX, currZ];
        restrictedMove(1);
        mapDrop();
        checkHeightIncrease();
    }

    void checkHeightIncrease()
    {
        int relativeMaxHeight = GameSettings.playerScore - blockSpawner.amountMapDropped;
        if (currY > relativeMaxHeight)
        {
            int heightIncrease = currY - relativeMaxHeight;
            if (GameSettings.playerScore > 3)
            {
                blockSpawner.shiftMapUp(heightIncrease);
            }
            GameSettings.playerScore += heightIncrease;
        }
    }

    public void mapDrop()
    {
        if (amountMapDropped < blockSpawner.amountMapDropped)
        {
            int dropDifference = blockSpawner.amountMapDropped - amountMapDropped;
            Vector3 newPos = transform.position - Vector3.up * GameSettings.blockSize * dropDifference;
            rb.MovePosition(newPos);
            amountMapDropped = blockSpawner.amountMapDropped;
            currY -= dropDifference;
        }
    }
    void restrictedMove(int jumpHeight)
    {
        switch(gameCam.currPos) 
        {
            case 0:
                restrictedMove0(jumpHeight);
                break;

            case 1:
                restrictedMove1(jumpHeight);
                break;

            case 2:
                restrictedMove2(jumpHeight);
                break;

            case 3:
                restrictedMove3(jumpHeight);
                break;
        }
        GameSettings.playerX = currX;
        GameSettings.playerY = currY;
        GameSettings.playerZ = currZ;
    }
    void restrictedMove0(int jumpHeight)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            addZ(jumpHeight);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            subZ(jumpHeight);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            addX(jumpHeight);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            subX(jumpHeight);
        }
    }
    void restrictedMove1(int jumpHeight)
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            addZ(jumpHeight);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            subZ(jumpHeight);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            addX(jumpHeight);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            subX(jumpHeight);
        }
    }
    void restrictedMove2(int jumpHeight)
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            addZ(jumpHeight);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            subZ(jumpHeight);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            addX(jumpHeight);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            subX(jumpHeight);
        }
    }
    void restrictedMove3(int jumpHeight)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            addZ(jumpHeight);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            subZ(jumpHeight);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            addX(jumpHeight);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            subX(jumpHeight);
        }
    }
    void addZ(int jumpHeight)
    {
        if (currZ + 1 < blockSpawner.gridZ)
        {
            int targetHeight = blockSpawner.heightMap[currX, currZ + 1];
            if (targetHeight <= currY + jumpHeight)
            {
                currZ++;
                rb.MovePosition(blockSpawner.gridToCoords(currX, targetHeight, currZ));
            }
        }
    }
    void subZ(int jumpHeight)
    {
        if (currZ - 1 >= 0)
        {
            int targetHeight = blockSpawner.heightMap[currX, currZ - 1];
            if (targetHeight <= currY + jumpHeight)
            {
                currZ--;
                rb.MovePosition(blockSpawner.gridToCoords(currX, targetHeight, currZ));
            }
        }
    }
    void addX(int jumpHeight)
    {
        if (currX + 1 < blockSpawner.gridX)
        {
            int targetHeight = blockSpawner.heightMap[currX + 1, currZ];
            if (targetHeight <= currY + jumpHeight)
            {
                currX++;
                rb.MovePosition(blockSpawner.gridToCoords(currX, targetHeight, currZ));
            }
        }
    }
    void subX(int jumpHeight)
    {
        if (currX - 1 >= 0)
        {
            int targetHeight = blockSpawner.heightMap[currX - 1, currZ];
            if (targetHeight <= currY + jumpHeight)
            {
                currX--;
                rb.MovePosition(blockSpawner.gridToCoords(currX, targetHeight, currZ));
            }
        }
    }

}
