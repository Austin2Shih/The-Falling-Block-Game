using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skunk : MonoBehaviour
{

    public float sprayCooldown;
    private float prevSpray;
    bool canSpray;
    public float sprayLingerTime;
    GameObject currSpray;
    private int despawnHeight = -10; //distance from platform that bird will despawn at

    private int x, z;

    ObjectPooler objectPooler;
    EnemySpawner enemySpawner;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = EnemySpawner.Instance;
        objectPooler = ObjectPooler.Instance;
        rb = this.GetComponent<Rigidbody>();
        prevSpray = 0;//-acornCooldown;
        canSpray = true;

    }

    // Update is called once per frame
    void Update()
    {
        processSprayAction();
        checkDespawn();
    }

    public void spawn(int x, int z)
    {
        this.x = x;
        this.z = z;
        canSpray = true;
        prevSpray = Time.fixedTime;
    }

    private void despawnSkunk()
    {
        enemySpawner.despawnEnemy(this.gameObject, "Skunk");
        enemySpawner.despawnEnemy(currSpray, "Spray");
    }

    private void checkDespawn()
    {
        float currY = transform.position.y;
        if (currY < (despawnHeight * GameSettings.blockSize))
        {
            despawnSkunk();
        }
    }

    private void processSprayAction()
    {
        if (canSpray)
        {
            if (GameSettings.playerX == this.x)
            {
                if (GameSettings.playerZ == this.z + 1)
                {
                    spray(x, z + 1);
                } else if (GameSettings.playerZ == this.z - 1)
                {
                    spray(x, z - 1);
                }

            } else if (GameSettings.playerZ == this.z)
            {
                if (GameSettings.playerX == this.x + 1)
                {
                    spray(x + 1, z);
                }
                else if (GameSettings.playerX == this.x - 1)
                {
                    spray(x - 1, z);
                }
            }
        } else if (Time.fixedTime - prevSpray >= sprayCooldown)
        {
            canSpray = true;
        }
    }

    private void spray(int x, int z)
    {
        objectPooler = ObjectPooler.Instance;
        prevSpray = Time.fixedTime;
        canSpray = false;
        Vector3 spawnLocation = gridToCoords(x, 0, z);
        GameObject spawnedSpray = objectPooler.SpawnFromPool("Spray", spawnLocation, Quaternion.identity);
        currSpray = spawnedSpray;
        currSpray.GetComponent<Spray>().setSprayLinger(sprayLingerTime);
        currSpray.GetComponent<Spray>().spray();
    }

    //Unity's built in on Collision for colliders set as Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            despawnSkunk();
        }
    }

    private Vector3 gridToCoords(int x, int y, int z)
    {
        Vector3 outputVector = new Vector3(x, y, z) * GameSettings.blockSize;
        outputVector += new Vector3(GameSettings.blockSize / 2.0f, GameSettings.blockSize / 2.0f, GameSettings.blockSize / 2.0f);
        return outputVector;
    }

}
