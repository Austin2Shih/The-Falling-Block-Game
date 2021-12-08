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
        GameObject spawnedBlock = objectPooler.SpawnFromPool("Block", spawnLocation, Quaternion.identity);

        spawnedBlock.SetActive(true);

        Rigidbody rb;
        rb = spawnedBlock.GetComponent<Rigidbody>();
        rb.velocity = -0.01f * Vector3.up;
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
}
