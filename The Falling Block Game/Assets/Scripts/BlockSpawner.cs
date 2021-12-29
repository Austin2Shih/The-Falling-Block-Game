using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public float spawnDelay;
    public float spawnHeight;
    public int gridX;
    public int gridY;
    public int gridZ;

    ObjectPooler objectPooler;
    private float prevSpawn;
    private float prevMapDropReset;
    public int amountMapDropped = 0;
    private float blockSize;
    public int[,] grid;
    public GameObject[,,] blockMatrix;
    public int matrixFloor;
    public float despawnHeight;


    public static BlockSpawner Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        prevSpawn = Time.fixedTime;
        prevMapDropReset = Time.fixedTime;
        blockSize = GameSettings.blockSize;
        grid = new int[gridX, gridZ];
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridZ; j++)
            {
                grid[i, j] = 0;
            }
        }
        blockMatrix = new GameObject[gridY, gridX, gridZ];
        for (int i = 0; i < gridY; i++)
        {
            for (int j = 0; j < gridX; j++)
            {
                for (int k = 0; k < gridZ; k++)
                {
                    blockMatrix[i, j, k] = null;
                }
            }
        }
        matrixFloor = 0;
        despawnHeight = gridHeightToWorldHeight(-1);
    }

    // Update is called once per frame
    void Update()
    {
        float timeForFrame = Time.fixedTime;
        if ((timeForFrame >= prevSpawn + spawnDelay))
        {
            prevSpawn = timeForFrame;
            StartCoroutine(spawnGridFromLinesZ(1, 1, 0.5f, 0.5f));
        }

        updateMapDrop();
    }

    void spawnBlock(int x, int z)
    {
        Vector3 spawnLocation = new Vector3(x, spawnHeight, z) * blockSize;
        spawnLocation += new Vector3(blockSize / 2, 0, blockSize / 2);
        GameObject spawnedBlock = objectPooler.SpawnFromPool("Block", spawnLocation, Quaternion.identity);

        spawnedBlock.GetComponent<Block>().spawn(x, z);
    }

    void spawnRandom()
    {
        int x = Random.Range(0, gridX);
        int z = Random.Range(0, gridZ);
        spawnBlock(x, z);
    }

    public void despawnBlock(GameObject block)
    {
        block.SetActive(false);
        objectPooler.poolDict["Block"].Enqueue(block);
    }

    public void setBlockMatrix(int y, int x, int z, GameObject block)
    {
        int matrixY = (y + matrixFloor) % gridY;
        blockMatrix[matrixY, x, z] = block;
    }

    public GameObject getBlockMatrix(int y, int x, int z)
    {
        int matrixY = (y + matrixFloor) % gridY;
        return blockMatrix[matrixY, x, z];
    }

    public int getBlockMatrixY(int y)
    {
        Debug.Log("rawY: " + y + " processedY: " + ((y + (gridY) - matrixFloor) % gridY) + " matrix floor: " + matrixFloor);
        return (y + (gridY) - matrixFloor) % gridY;
    }

    public void printGrid()
    {
        string debugMessage = "\n";
        for (int i = 0; i < gridX; i++)
        {
            debugMessage += "[";
            for (int j = 0; j < gridZ - 1; j++)
            {
                debugMessage += grid[i, j] + ", ";
            }
            debugMessage += grid[i, gridZ - 1] + "]\n";
        }
        Debug.Log(debugMessage);
    }

    public void removeGridLayer()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridZ; j++)
            {
                if (grid[i, j] != 0)
                {
                    grid[i, j]--;
                }
            }
        }
    }

    public Vector3 gridToCoords(int x, int y, int z)
    {
        Vector3 outputVector = new Vector3(x, y, z) * blockSize;
        outputVector += new Vector3(blockSize / 2.0f, blockSize / 4.0f, blockSize / 2.0f);
        return outputVector;
    }

    public float gridHeightToWorldHeight(int gridHeight)
    {
        return blockSize / 2.0f + gridHeight * blockSize;
    }

    public void incrementDespawnHeight()
    {
        Debug.Log(matrixFloor);
        if (matrixFloor < gridY - 1)
        {
            matrixFloor += 1;
        }
        else
        {
            matrixFloor = 0;
        }
        despawnHeight += blockSize;
        spawnHeight += blockSize;
    }

    private void updateMapDrop()
    {
        float dTime = Time.fixedTime - prevMapDropReset;
        if (dTime >= GameSettings.mapDropPeriod)
        {
            amountMapDropped++;
            prevMapDropReset = Time.fixedTime;
            removeGridLayer();
        }
    }

    private IEnumerator spawnLineZ(int positionZ, int direction, float timeBetweenSpawns)
    {
        if (direction > 0)
        {
            for (int i = 0; i < gridX; i++)
            {
                spawnBlock(i, positionZ);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
        else
        {
            for (int i = gridX - 1; i >= 0; i--)
            {
                spawnBlock(i, positionZ);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
    }

    private IEnumerator spawnGridFromLinesZ(int directionX, int directionZ, float timeBetweenSpawns, float timeBetweenLines)
    {
        if (directionX > 0)
        {
            for (int i = 0; i < gridZ; i++)
            {
                StartCoroutine(spawnLineZ(i, directionZ, timeBetweenSpawns));
                yield return new WaitForSeconds(timeBetweenLines);
            }
        }
        else
        {
            for (int i = gridX - 1; i >= 0; i--)
            {
                StartCoroutine(spawnLineZ(i, directionZ, timeBetweenSpawns));
                yield return new WaitForSeconds(timeBetweenLines);
            }
        }
    }


}
