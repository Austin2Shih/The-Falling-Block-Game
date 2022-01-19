using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirrel : MonoBehaviour
{

    public float acornCooldown;
    public float acornSpeed;
    private Vector3 acornVelocity;
    private float prevAcornToss;
    GameObject currAcorn;
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
        prevAcornToss = 0;//-acornCooldown;
        
    }

    // Update is called once per frame
    void Update()
    {
        processAcornAction();
        checkDespawn();
    }

    public void spawn(int x, int z, char coordLine, int moveDirection)
    {
        this.x = x;
        this.z = z;
        float xVel = 0;
        float zVel = 0;
        if (coordLine == 'x')
        {
            xVel = moveDirection * acornSpeed;
        }
        else if (coordLine == 'z')
        {
            zVel = moveDirection * acornSpeed;
        }
        acornVelocity = new Vector3(xVel, 0, zVel);
        prepareAcorn();
    }
    
    private void despawnSquirrel()
    {
        enemySpawner.despawnEnemy(this.gameObject, "Squirrel");
        enemySpawner.despawnEnemy(currAcorn, "Acorn");
    }

    private void checkDespawn()
    {
        float currY = transform.position.y;
        if (currY < (despawnHeight * GameSettings.blockSize))
        {
            despawnSquirrel();
        }
    }

    private void processAcornAction()
    {
        if (Time.fixedTime - prevAcornToss >= acornCooldown)
        {
            tossAcorn();
            prepareAcorn();
            prevAcornToss = Time.fixedTime;
        } 
    }

    private void prepareAcorn()
    {
        objectPooler = ObjectPooler.Instance;
        Vector3 spawnLocation = this.transform.position + acornVelocity.normalized;
        GameObject spawnedAcorn = objectPooler.SpawnFromPool("Acorn", spawnLocation, Quaternion.identity);
        spawnedAcorn.GetComponent<Acorn>().initAcorn(acornVelocity);
        currAcorn = spawnedAcorn;
    }

    private void tossAcorn()
    {
        currAcorn.GetComponent<Acorn>().tossAcorn();
    }

    //Unity's built in on Collision for colliders set as Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            despawnSquirrel();
        }
    }

}
