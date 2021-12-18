using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedThirdPersonCam : MonoBehaviour
{
    public static FixedThirdPersonCam Instance;
    private void Awake()
    {
        Instance = this;
    }

    PlayerMovement player;
    BlockSpawner blockSpawner;

    public float offSet;
    public float camHeight;
    int gridX;
    int gridZ;
    float blockSize;
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

        currPos = 0;
        transform.position = getCamPosition(currPos);
        transform.LookAt(player.transform.position);
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
            }
            else
            {
                currPos--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currPos = (currPos + 1) % 4;
        }

        Vector3 playerHeightShift = (Vector3.up * player.currY * GameSettings.blockSize);
        transform.position = getCamPosition(currPos) + playerHeightShift;
        transform.LookAt(player.transform.position);
    }

    private Vector3 getCamPosition(int position)
    {
        Vector3 playerPos = player.transform.position;
        switch(position)
        {
            case 1:
                return new Vector3(playerPos.x + blockSize * offSet, blockSize * camHeight, playerPos.z);
                break;
            case 2:
                return new Vector3(playerPos.x, blockSize * camHeight, playerPos.z + blockSize * offSet);
                break;
            case 3:
                return new Vector3(playerPos.x - blockSize * offSet, blockSize * camHeight, playerPos.z);
                break;
            default:
                return new Vector3(playerPos.x, blockSize * camHeight, playerPos.z - blockSize * offSet);
                break;
        }
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
