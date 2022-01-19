using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int spawnHeight = 0;
    public int spawnOffset = 4;
    private int gridX;
    private int gridZ;

    ObjectPooler objectPooler;
    private float blockSize;

    public static EnemySpawner Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gridX = GameSettings.gridX;
        gridZ = GameSettings.gridZ;
        objectPooler = ObjectPooler.Instance;
        blockSize = GameSettings.blockSize;
    }

    // Update is called once per frame
    void Update()
    {
        spawnRandomBird();
        spawnRandomSquirrel();
        spawnRandomSkunk();
    }

    /*     (This is the bird's path from spawn) 
      z ^     v 
        4 * * | * *
        3 * * | * *
        2 * * | * *
        1 * * | * *
        0 * * v * *
          0 1 2 3 4 --> x

        coordinateLine either x or z, whichever axis the bird's path is parallel to

        moveDirection is if the bird's path is in the positive 
        or negative direction of coordinateLine (1 or -1)

        coordinateValue is which x or z value the line should be spawned on (in this case it is 2)
    */
    public void spawnBird(char coordinateLine, int moveDirection, int coordinateValue)
    {
        int x = 0;
        int z = 0;
        if (coordinateLine == 'z')
        {
            x = coordinateValue;
            if (moveDirection == 1)
            {
                z = -spawnOffset;
            } else
            {
                z = gridZ + spawnOffset;
            }
        } else if (coordinateLine == 'x')
        {
            z = coordinateValue;
            if (moveDirection == 1)
            {
                x = -spawnOffset;
            }
            else
            {
                x = gridZ + spawnOffset;
            }
        } else
        {
            Debug.Log("Invalid bird coordinateLine");
        }

        Vector3 spawnLocation = gridToCoords(x, spawnHeight, z);
        GameObject spawnedBird = objectPooler.SpawnFromPool("Bird", spawnLocation, Quaternion.identity);
        spawnedBird.GetComponent<Bird>().spawn(coordinateLine, moveDirection);
    }

    /*     (This is the squirrel's acorn path from spawn) 
      z ^     v 
        4 * * | * *
        3 * * | * *
        2 * * | * *
        1 * * | * *
        0 * * v * *
          0 1 2 3 4 --> x

        coordinateLine either x or z, whichever axis the bird's path is parallel to

        firingDirection is if the bird's path is in the positive 
        or negative direction of coordinateLine (1 or -1)
    */
    public void spawnSquirrel(int x, int z, char coordinateLine, int firingDirection)
    {
        Vector3 spawnLocation = gridToCoords(x, spawnHeight, z);
        GameObject spawnedSquirrel = objectPooler.SpawnFromPool("Squirrel", spawnLocation, Quaternion.identity);
        spawnedSquirrel.GetComponent<Squirrel>().spawn(x, z, coordinateLine, firingDirection);
    }

    public void spawnSkunk(int x, int z)
    {
        Vector3 spawnLocation = gridToCoords(x, spawnHeight, z);
        GameObject spawnedSquirrel = objectPooler.SpawnFromPool("Skunk", spawnLocation, Quaternion.identity);
        spawnedSquirrel.GetComponent<Skunk>().spawn(x, z);
    }

    public void despawnEnemy(GameObject enemy, string poolTag)
    {
        enemy.SetActive(false);
        objectPooler.poolDict[poolTag].Enqueue(enemy);
    }
    private Vector3 gridToCoords(int x, int y, int z)
    {
        Vector3 outputVector = new Vector3(x, y, z) * blockSize;
        outputVector += new Vector3(blockSize / 2.0f, blockSize / 2.0f, blockSize / 2.0f);
        return outputVector;
    }

    private void spawnRandomBird()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int coordLineInt = Random.Range(0, 2);
            char coordLine = (coordLineInt == 0) ? 'x' : 'z';

            int moveDirInt = Random.Range(0, 2);
            int moveDir = (moveDirInt == 0) ? -1 : 1;
            int coordValue;
            if (coordLine == 'x')
            {
                coordValue = Random.Range(0, GameSettings.gridX);
            } else
            {
                coordValue = Random.Range(0, GameSettings.gridZ);
            }
            spawnBird(coordLine, moveDir, coordValue);
        }
    }

    private void spawnRandomSquirrel()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int coordLineInt = Random.Range(0, 2);
            char coordLine = (coordLineInt == 0) ? 'x' : 'z';

            int moveDirInt = Random.Range(0, 2);
            int moveDir = (moveDirInt == 0) ? -1 : 1;
            int spawnX, spawnZ;
            spawnX = Random.Range(0, GameSettings.gridX);
            spawnZ = Random.Range(0, GameSettings.gridZ);
            spawnSquirrel(spawnX, spawnZ, coordLine, moveDir);
        }
    }
    private void spawnRandomSkunk()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int spawnX, spawnZ;
            spawnX = Random.Range(0, GameSettings.gridX);
            spawnZ = Random.Range(0, GameSettings.gridZ);
            spawnSkunk(spawnX, spawnZ);
        }
    }

}
