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

    public GameObject block;

    ObjectPooler objectPooler;
    private float prevSpawn;
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
        blockSize = block.transform.localScale.x;
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
    }

    void spawnRandom()
    {
        int x = Random.Range(0, gridX);
        int z = Random.Range(0, gridZ);
        Vector3 spawnLocation = new Vector3(x, spawnHeight, z) * blockSize;
        spawnLocation += new Vector3(blockSize / 2, 0, blockSize / 2);
        GameObject spawnedBlock = objectPooler.SpawnFromPool("Block", spawnLocation, Quaternion.identity);

        spawnedBlock.SetActive(true);
        spawnedBlock.GetComponent<Block>().spawn(x, z);
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

    public Vector3 gridToCoords(int x, int y, int z)
    {
        Vector3 outputVector = new Vector3(x, y, z) * blockSize;
        outputVector += new Vector3(blockSize / 2.0f, blockSize / 2.0f, blockSize / 2.0f);
        return outputVector;
    }

    public float getBlockSize()
    {
        return blockSize;
    }
}
