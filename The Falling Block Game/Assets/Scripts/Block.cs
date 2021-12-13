using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float fallAccel = -9.8f;

    Rigidbody rb;
    int blockSize;
    bool stable; //variable indicating if the block has settled or not.

    private float timeStartFall;
    private float fallVelocity;
    private int currX;
    private int currZ;
    private float despawnHeight;


    BlockSpawner blockSpawner;
    void Start()
    {
        blockSpawner = BlockSpawner.Instance;
        rb = this.GetComponent<Rigidbody>();
        blockSize = (int)this.transform.localScale.x;
        despawnHeight = gridHeightToWorldHeight(-1);
    }

    private void FixedUpdate()
    {
        fallFromSpawn();
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool hitBlock = collision.gameObject.tag == "Building Block";
        float blockBottomY = this.transform.position.y - 1;
        float collisionY = collision.GetContact(0).point.y;
        bool landed = Mathf.Abs(collisionY - blockBottomY) < 0.2;

        if (hitBlock && landed)
        {
            int x = (int)this.transform.position.x / blockSize;
            int z = (int)this.transform.position.z / blockSize;
            blockSpawner.grid[x, z]++;
            blockSpawner.printGrid();
        }
    }

    public void spawn(int x, int z)
    {
        fallVelocity = 0;
        timeStartFall = Time.fixedTime;
        currX = x;
        currZ = z;
        stable = false;
    }
    private void fallFromSpawn()
    {
        if (stable)
        {
            return;
        }
        int destinationHeight = blockSpawner.grid[currX, currZ];
        float worldDestHeight = gridHeightToWorldHeight(destinationHeight);
        Vector3 currPos = transform.position;
        if (currPos.y <= worldDestHeight)
        {
            transform.position = new Vector3(currPos.x, worldDestHeight, currPos.z);
            stable = true;
            fallVelocity = 0;
            blockSpawner.grid[currX, currZ]++;
        } else
        {
            float timeFallen = Time.fixedTime - timeStartFall;
            fallVelocity = fallAccel * timeFallen;
        }

        rb.velocity = Vector3.up * fallVelocity;
    }

    private void deconstructBottom()
    {

        if (transform.position.y <= despawnHeight)
        {
            blockSpawner.despawnBlock(this.GetComponent<GameObject>());
        }
    }

    private float gridHeightToWorldHeight(int gridHeight)
    {
        return blockSize / 2 + gridHeight * blockSize;
    }
}
