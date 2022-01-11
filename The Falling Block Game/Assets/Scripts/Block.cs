using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// block
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
    private int currY;
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
        stable = true;
        fallVelocity = 0;

    }

    private void FixedUpdate()
    {
        fallFromSpawn();
        mapDrop();
        deconstructBottom();
        clearObstruction();
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
        int destinationHeight = blockSpawner.heightMap[currX, currZ];
        float worldDestHeight = blockSpawner.gridHeightToWorldHeight(destinationHeight);
        Vector3 currPos = transform.position;

        amountMapDropped = blockSpawner.amountMapDropped;

        if (currPos.y <= worldDestHeight)
        {
            Vector3 newPosition = new Vector3(currPos.x, worldDestHeight, currPos.z);
            rb.MovePosition(newPosition);
            stable = true;
            currY = destinationHeight;
            
            fallVelocity = 0;
            int currTowerHeight = blockSpawner.heightMap[currX, currZ];
            blockSpawner.heightMap[currX, currZ]++;
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
        if (amountMapDropped < blockSpawner.amountMapDropped)
        {
            int dropDifference = blockSpawner.amountMapDropped - amountMapDropped;
            Vector3 newPos = transform.position - Vector3.up * GameSettings.blockSize * dropDifference;
            rb.MovePosition(newPos);
            amountMapDropped = blockSpawner.amountMapDropped;
            currY -= dropDifference;
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

    public void clearObstruction()
    {
        switch(GameSettings.camPos)
        {
            case 0:
                if (currZ < GameSettings.playerZ && currY >= GameSettings.playerY)
                {
                    setOpaque();
                } else
                {
                    setSolid();
                }
                break;

            case 1:
                if (currX > GameSettings.playerX && currY >= GameSettings.playerY)
                {
                    setOpaque();
                }
                else
                {
                    setSolid();
                }
                break;

            case 2:
                if (currZ > GameSettings.playerZ && currY >= GameSettings.playerY)
                {
                    setOpaque();
                }
                else
                {
                    setSolid();
                }
                break;

            case 3:
                if (currX < GameSettings.playerX && currY >= GameSettings.playerY)
                {
                    setOpaque();
                }
                else
                {
                    setSolid();
                }
                break;
        }
    }

}
