using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public static int spawnHeight = 15;
    public static int blockSize = 1;

    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;

    // Start is called before the first frame update
    void Start()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDict.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, int x, int z)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " does not exist.");
            return null;
        }

        GameObject objectToSpawn = poolDict[tag].Dequeue();
        Rigidbody rb;

        objectToSpawn.SetActive(true);
        
        Vector3 newPos = new Vector3(x * blockSize, spawnHeight, z * blockSize);
        objectToSpawn.transform.position = newPos;

        rb = objectToSpawn.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
  
        poolDict[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
