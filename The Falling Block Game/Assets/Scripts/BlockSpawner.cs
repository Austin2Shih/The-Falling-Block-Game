using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public float spawnDelay;
    public int spawnHeight;
    public int gridX;
    public int gridZ;

    ObjectPooler objectPooler;
    private float prevSpawn;
    private float prevMapDropReset;
    public int amountMapDropped = 0;
    private float blockSize;
    public int[,] grid;

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
    }

    // Update is called once per frame
    void Update()
    {
        float timeForFrame = Time.fixedTime;
        if (timeForFrame >= prevSpawn + spawnDelay)
        {
            prevSpawn = timeForFrame;
            spawnRandom();
        }

        updateMapDrop();
    }

    void spawnRandom()
    {
        int x = Random.Range(0, gridX);
        int z = Random.Range(0, gridZ);
        Vector3 spawnLocation = new Vector3(x, spawnHeight, z) * blockSize;
        spawnLocation += new Vector3(blockSize / 2, 0, blockSize / 2);
        GameObject spawnedBlock = objectPooler.SpawnFromPool("Block", spawnLocation, Quaternion.identity);

        spawnedBlock.GetComponent<Block>().spawn(x, z);
    }

    public void despawnBlock(GameObject block)
    {
        block.SetActive(false);
        objectPooler.poolDict["Block"].Enqueue(block);
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
        outputVector += new Vector3(blockSize / 2.0f, blockSize / 2.0f, blockSize / 2.0f);
        return outputVector;
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

}
