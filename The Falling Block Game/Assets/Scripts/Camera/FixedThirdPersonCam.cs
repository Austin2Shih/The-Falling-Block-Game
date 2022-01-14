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
    MapSpawner mapSpawner;

    public float offSet;
    public float camHeight;
    int gridX;
    int gridZ;
    float blockSize;
    public int currPos;

    private Vector3 prevPos;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMovement.Instance;
        mapSpawner = MapSpawner.Instance;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gridX = GameSettings.gridX;
        gridZ = GameSettings.gridZ;
        blockSize = GameSettings.blockSize;

        currPos = 0;
        transform.position = getCamPosition(currPos);
        prevPos = transform.position;
        transform.LookAt(player.transform.position);
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
        GameSettings.camPos = currPos;

        transform.position = getCamPosition(currPos);
        transform.LookAt(player.transform.position);
    }

    private Vector3 getCamPosition(int position)
    {
        Vector3 playerPos = player.transform.position;
        switch (position)
        {
            case 1:
                return new Vector3(playerPos.x + blockSize * offSet, blockSize * camHeight, playerPos.z);
            case 2:
                return new Vector3(playerPos.x, blockSize * camHeight, playerPos.z + blockSize * offSet);
            case 3:
                return new Vector3(playerPos.x - blockSize * offSet, blockSize * camHeight, playerPos.z);
            default:
                return new Vector3(playerPos.x, blockSize * camHeight, playerPos.z - blockSize * offSet);
        }
    }

    private bool camChangeDetected()
    {
        if (this.transform.position == prevPos)
        {
            return false;
        } else
        {
            prevPos = this.transform.position;
            return true;
        }
    }

}
