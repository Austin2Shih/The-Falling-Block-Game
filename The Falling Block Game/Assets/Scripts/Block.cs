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
    private int amountMapDropped = 0;
    private float timeStartDespawn;
    private float despawnTime;


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
        mapDrop();
        deconstructBottom();
            
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
        timeStartDespawn = 0;
        despawnTime = 0;
        amountMapDropped = 0;
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

        amountMapDropped = blockSpawner.amountMapDropped;

        if (currPos.y <= worldDestHeight)
        {
            Vector3 newPosition = new Vector3(currPos.x, worldDestHeight, currPos.z);
            transform.position = newPosition;
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

    public void mapDrop()
    {   
        if (stable && amountMapDropped < blockSpawner.amountMapDropped)
        {
            amountMapDropped++;
            transform.Translate(Vector3.up * -blockSize);
        }
    }

    private void deconstructBottom()
    {
        if (timeStartDespawn == 0)
        {
            if (transform.position.y - 0.1 <= despawnHeight)
            {
                timeStartDespawn = Time.fixedTime;
                despawnTime = timeStartDespawn + Random.Range(0.5f, 3.5f);
            }
        } else if (Time.fixedTime >= despawnTime)
        {
            blockSpawner.despawnBlock(gameObject);
        }

    }

    private float gridHeightToWorldHeight(int gridHeight)
    {
        return blockSize / 2.0f + gridHeight * blockSize;
    }
}
