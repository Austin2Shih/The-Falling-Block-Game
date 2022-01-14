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

    public int x, y, z;
    float moveDist;

    Rigidbody rb;
    MapSpawner blockSpawner;
    FixedThirdPersonCam gameCam;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        blockSpawner = MapSpawner.Instance;
        gameCam = FixedThirdPersonCam.Instance;
        moveDist = GameSettings.blockSize;
        rb.MovePosition(new Vector3(moveDist / 2.0f, moveDist / 4.0f, moveDist / 2.0f));
        x = 0;
        y = 0;
        z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        restrictedMove();
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
        GameSettings.playerX = x;
        GameSettings.playerY = y;
        GameSettings.playerZ = z;
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
        if (z + 1 < GameSettings.gridZ)
        {
            z += 1;
            rb.MovePosition(blockSpawner.gridToCoords(x, y, z));
        }
    }
    void subZ()
    {
        if (z - 1 >= 0)
        {
            z -= 1;
            rb.MovePosition(blockSpawner.gridToCoords(x, y, z));
        }
    }
    void addX()
    {
        if (x + 1 < GameSettings.gridX)
        {
            x += 1;
            rb.MovePosition(blockSpawner.gridToCoords(x, y, z));
        }
    }
    void subX()
    {
        if (x - 1 >= 0)
        {
            x -= 1;
            rb.MovePosition(blockSpawner.gridToCoords(x, y, z));
        }
    }

}
