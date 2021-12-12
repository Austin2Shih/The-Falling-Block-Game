using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCam : MonoBehaviour
{
    //BlockSpawner blockSpawner;
    public float offSet;
    public float camHeight;
    public float centerHeight;
    int gridX = 7;
    int gridZ = 7;
    float blockSize = 2;
    Vector3[] positions;
    Vector3 center;
    int currPos;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        /*
        blockSpawner = BlockSpawner.Instance;
        gridX = blockSpawner.gridX;
        gridZ = blockSpawner.gridZ;
        blockSize = blockSpawner.getBlockSize();
        */
        positions = new Vector3[4];
        positions[0] = new Vector3((gridX / 2.0f) * blockSize, blockSize * camHeight, blockSize * -offSet);
        positions[1] = new Vector3(blockSize * (gridX + offSet), blockSize * camHeight, (gridZ / 2.0f) * blockSize);
        positions[2] = new Vector3((gridX / 2.0f) * blockSize, blockSize * camHeight, blockSize * (gridZ + offSet));
        positions[3] = new Vector3(blockSize * -offSet, blockSize * camHeight, (gridZ / 2.0f) * blockSize);

        center = new Vector3((gridX / 2.0f) * blockSize, blockSize * centerHeight, (gridZ / 2.0f) * blockSize);

        currPos = 0;
        transform.position = positions[currPos];
        transform.LookAt(center);
    }

    // Update is called once per frame
    void Update()
    {
        updatePosition();
    }

    void updatePosition()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currPos == 0)
            {
                currPos = 3;
            } else
            {
                currPos--;
            }
        } else if (Input.GetKeyDown(KeyCode.RightArrow)){
            currPos = (currPos + 1) % 4;
        }
        transform.position = positions[currPos];
        transform.LookAt(center);
    }
}
