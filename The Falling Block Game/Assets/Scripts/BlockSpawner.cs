using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject spawnLocationPrefab;
    public float spawnDelay;
    public float spawnHeight;
    public int gridX;
    public int gridY;
    public int gridZ;

    ObjectPooler objectPooler;
    private float prevSpawn;
    public int amountMapDropped = 0;
    private float blockSize;
    public int[,] heightMap;
    public GameObject[,,] blockMatrix;
    public int matrixFloor;
    public float despawnHeight;

    private GameObject[,] spawners;


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
        blockSize = GameSettings.blockSize;
        heightMap = new int[gridX, gridZ];
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridZ; j++)
            {
                heightMap[i, j] = 0;
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
        spawners = new GameObject[gridX, gridZ];
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridZ; j++)
            {
                spawners[i, j] = Instantiate(spawnLocationPrefab);
                spawners[i, j].GetComponent<SpawnLocation>().setPos(i, j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float timeForFrame = Time.fixedTime;

        if ((timeForFrame >= prevSpawn + spawnDelay))
        {
            prevSpawn = timeForFrame;
            //StartCoroutine(spawnGridFromLinesZ(-1, 1, 1f, 1f));
            StartCoroutine(spawnRandomOnTimer(10, 3, 0.5f));
        }

        updateMapDrop();
    }

    void spawnBlock(int x, int z)
    {
        Vector3 spawnLocation = new Vector3(x, spawnHeight, z) * blockSize;
        spawnLocation += new Vector3(blockSize / 2, 0, blockSize / 2);
        GameObject spawnedBlock = objectPooler.SpawnFromPool("Block", spawnLocation, Quaternion.identity);

        spawners[x, z].GetComponent<SpawnLocation>().spawn(spawnedBlock);
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
                debugMessage += heightMap[i, j] + ", ";
            }
            debugMessage += heightMap[i, gridZ - 1] + "]\n";
        }
        Debug.Log(debugMessage);
    }

    public void removeGridLayers(int amount)
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridZ; j++)
            {
                if (heightMap[i, j] != 0)
                {
                    heightMap[i, j] -= amount;
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

    public void shiftMapUp(int amount)
    {
        if (matrixFloor < gridY - amount)
        {
            matrixFloor += amount;
        }
        else
        {
            matrixFloor = (matrixFloor + amount) % gridY;
        }
        despawnHeight += blockSize * amount;
        spawnHeight += blockSize * amount;
    }

    private void updateMapDrop()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            int minHeight = minHeightMap();
            int dropSize = (minHeight >= 3)? (minHeight - 3): minHeight;
            amountMapDropped += dropSize;
            removeGridLayers(dropSize);
            spawnHeight -= dropSize;
            despawnHeight -= dropSize * blockSize;
        }
    }

    private int minHeightMap()
    {
        int min = heightMap[0, 0];
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridZ; j++)
            {
                if (heightMap[i, j] < min)
                {
                    min = heightMap[i, j];
                }
            }
        }
        return min;
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

    private IEnumerator spawnRandomOnTimer(int spawnCount, int blocksPerSpawn, float timeBetweenSpawns)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            for (int j = 0; j < blocksPerSpawn; j++)
            {
                spawnRandom();
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        
    }


}
