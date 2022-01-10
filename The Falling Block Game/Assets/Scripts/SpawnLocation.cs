using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation: MonoBehaviour
{
    public float spawnCooldown = 0.64f;
    public float fillCooldown = 7;
    Queue<GameObject> queue;
    float prevSpawn;
    int x, z;
    float prevFill;
    bool offFillCooldown;

    BlockSpawner blockSpawner;

    public void Start()
    {
        queue = new Queue<GameObject>();
        prevSpawn = Time.fixedTime;
        prevFill = Time.fixedTime;
        offFillCooldown = true;
        blockSpawner = BlockSpawner.Instance;
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
        updateFill();
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

    public void fillGap()
    {
        if (blockSpawner.maxHeightMap() - blockSpawner.heightMap[x, z] >= blockSpawner.maxGap)
        {
            offFillCooldown = false;
            prevFill = Time.fixedTime;
            StartCoroutine(waitRandomBeforeFill());
        }
    }

    private IEnumerator waitRandomBeforeFill()
    {
        float despawnTime = Random.Range(0.0f, 7f);
        yield return new WaitForSeconds(despawnTime);
        blockSpawner.spawnBlock(x, z);
    }

    private void updateFill()
    {
        if (offFillCooldown)
        {
            fillGap();
        } else if ((Time.fixedTime - prevFill) >= fillCooldown)
        {
            offFillCooldown = true;
        }
    }
}
