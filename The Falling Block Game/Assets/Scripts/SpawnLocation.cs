using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation: MonoBehaviour
{
    public float spawnCooldown = 0.64f;
    Queue<GameObject> queue;
    float prevSpawn;
    int x, z;

    public void Start()
    {
        queue = new Queue<GameObject>();
        prevSpawn = Time.fixedTime;
    }

    // Update is called once per frame
    public void Update()
    {
        if (queue.Count != 0)
        {
            if ((Time.fixedTime - prevSpawn) >= spawnCooldown)
            {
                queue.Dequeue().GetComponent<Block>().spawn(x, z);
                prevSpawn = Time.fixedTime;
            }
        }
    }

    public void spawn(GameObject block)
    {
        queue.Enqueue(block);
    }

    public void setPos(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}
