using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

public class MapSpawner : MonoBehaviour
{
    public int spawnHeight;
    private int gridX;
    private int gridZ;

    ObjectPooler objectPooler;
    private float blockSize;

    public static MapSpawner Instance;
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
        spawnPlatform();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawnPlatform()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridZ; j++)
            {
                spawnBlock(i, -1, j);
            }
        }
    }

    public void spawnBlock(int x, int y, int z)
    {
        Vector3 spawnLocation = gridToCoords(x, y, z);
        GameObject spawnedBlock = objectPooler.SpawnFromPool("Block", spawnLocation, Quaternion.identity);
        spawnedBlock.GetComponent<Block>().spawn(x, y, z);
    }

    public void despawnBlock(GameObject block)
    {
        block.SetActive(false);
        objectPooler.poolDict["Block"].Enqueue(block);
    }
    public Vector3 gridToCoords(int x, int y, int z)
    {
        Vector3 outputVector = new Vector3(x, y, z) * blockSize;
        outputVector += new Vector3(blockSize / 2.0f, blockSize / 2.0f, blockSize / 2.0f);
        return outputVector;
    }
}
