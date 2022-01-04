using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCam : MonoBehaviour
{
    public static GameCam Instance;
    private void Awake()
    {
        Instance = this;
    }

    PlayerMovement player;
    BlockSpawner blockSpawner;

    public float offSet;
    public float camHeight;
    public float centerHeight;
    int gridX;
    int gridZ;
    float blockSize;
    Vector3[] positions;
    Vector3 center;
    public int currPos;

    private int amountMapDropped = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMovement.Instance;
        blockSpawner = BlockSpawner.Instance;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gridX = GameSettings.gridX;
        gridZ = GameSettings.gridZ;
        blockSize = GameSettings.blockSize;
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
        mapDrop();
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

        Vector3 playerHeightShift = (Vector3.up * player.currY * GameSettings.blockSize);
        transform.position = positions[currPos] + playerHeightShift;
        transform.LookAt(center + playerHeightShift);
    }

    public void mapDrop()
    {
        if (amountMapDropped < blockSpawner.amountMapDropped)
        {
            if (player.transform.position.y > 1.01)
            {
                transform.Translate(Vector3.up * -GameSettings.blockSize);
            }
            amountMapDropped++;
        }
    }
}
