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

    private int x, y, z;

    Rigidbody rb;
    MapSpawner mapSpawner;
    FixedThirdPersonCam gameCam;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        mapSpawner = MapSpawner.Instance;
        gameCam = FixedThirdPersonCam.Instance;
        x = GameSettings.gridX / 2;
        y = 0;
        z = GameSettings.gridZ / 2;
        this.transform.position = gridToCoords(x, y, z);
    }

    // Update is called once per frame
    void Update()
    {
        restrictedMove();
    }

    public Vector3 gridToCoords(int x, int y, int z)
    {
        Vector3 outputVector = new Vector3(x, y, z) * GameSettings.blockSize;
        outputVector += new Vector3(GameSettings.blockSize / 2.0f, GameSettings.blockSize / 4.0f, GameSettings.blockSize / 2.0f);
        return outputVector;
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
            rb.MovePosition(gridToCoords(x, y, z));
        }
    }
    void subZ()
    {
        if (z - 1 >= 0)
        {
            z -= 1;
            rb.MovePosition(gridToCoords(x, y, z));
        }
    }
    void addX()
    {
        if (x + 1 < GameSettings.gridX)
        {
            x += 1;
            rb.MovePosition(gridToCoords(x, y, z));
        }
    }
    void subX()
    {
        if (x - 1 >= 0)
        {
            x -= 1;
            rb.MovePosition(gridToCoords(x, y, z));
        }
    }

}
