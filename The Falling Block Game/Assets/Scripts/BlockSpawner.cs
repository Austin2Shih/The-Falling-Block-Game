using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    ObjectPooler objectPooler;
    public static int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        count++;
        int x = Random.Range(0, 10);
        int z = Random.Range(0, 10);
        if (count == 60)
        {
            count = 0;
            objectPooler.SpawnFromPool("Block", x, z);
        }
    }
}
