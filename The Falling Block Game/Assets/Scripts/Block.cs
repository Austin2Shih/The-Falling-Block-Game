using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float fallAccel = -9.8f;
    public float opaqueAlpha = 0.3f;
    public float solidAlpha = 1.0f;

    Rigidbody rb;
    int blockSize;
    bool stable; //variable indicating if the block has settled or not.

    private float timeStartFall;
    private float fallVelocity;
    private int currX;
    private int currZ;
    private int amountMapDropped = 0;
    private float timeStartDespawn;
    private float despawnTime;


    BlockSpawner blockSpawner;
    void Start()
    {
        blockSpawner = BlockSpawner.Instance;
        rb = this.GetComponent<Rigidbody>();
        blockSize = GameSettings.blockSize;
        
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
        float worldDestHeight = blockSpawner.gridHeightToWorldHeight(destinationHeight);
        Vector3 currPos = transform.position;

        amountMapDropped = blockSpawner.amountMapDropped;

        if (currPos.y <= worldDestHeight)
        {
            Vector3 newPosition = new Vector3(currPos.x, worldDestHeight, currPos.z);
            rb.MovePosition(newPosition);
            stable = true;
            
            fallVelocity = 0;
            int currTowerHeight = blockSpawner.grid[currX, currZ];
            blockSpawner.grid[currX, currZ]++;
            blockSpawner.setBlockMatrix(currTowerHeight, currX, currZ, this.gameObject);
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
            Vector3 newPos = transform.position - Vector3.up * blockSize;
            rb.MovePosition(newPos);
        }
    }

    private void deconstructBottom()
    {
        if (timeStartDespawn == 0)
        {
            if (transform.position.y - 0.1 <= blockSpawner.despawnHeight)
            {
                timeStartDespawn = Time.fixedTime;
                despawnTime = timeStartDespawn + Random.Range(0.5f, 3.5f);
            }
        } else if (Time.fixedTime >= despawnTime)
        {
            blockSpawner.despawnBlock(gameObject);
        }

    }

    void ChangeAlpha(Material mat, float alphaVal)
    {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
        mat.SetColor("_Color", newColor);
    }

    public void setOpaque()
    {
        ChangeAlpha(gameObject.GetComponent<Renderer>().material, opaqueAlpha);
    }

    public void setSolid()
    {
        ChangeAlpha(gameObject.GetComponent<Renderer>().material, solidAlpha);
    }
}
